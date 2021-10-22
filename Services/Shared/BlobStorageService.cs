using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Api.Services.Shared
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly MultimediaSettings _multimediaSettings;

        public Uri Uri
        {
            get { return GetContainer().Uri; }
        }

        public BlobStorageService(IOptions<AzureStorageSettings> azureStorageOptions, 
            IOptions<MultimediaSettings> multimediaOptions)
        {
            _azureStorageSettings = azureStorageOptions.Value;
            _multimediaSettings = multimediaOptions.Value;
        }

        public void DeleteFileByUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            var container = GetContainer();
            var relativePath = GetRelativePath(container, url);

            DeleteFile(container, relativePath);
        }

        public async Task<string> MoveFileByUrl(string fileUrl, string destination)
        {
            if (string.IsNullOrEmpty(fileUrl) || string.IsNullOrEmpty(destination))
            {
                return "";
            }
            var container = await CreateContainerIfNotExists();
            var relativePath = GetRelativePath(container, fileUrl);
            var filename = fileUrl.Split('/').Last();
            filename = RemoveTimestamp(filename); // removing old timestamp

            return await MoveFile(container, relativePath, destination + filename, false);
        }

        public async Task<string> CopyFileByUrl(string fileUrl, string folder = "")
        {
            if (string.IsNullOrEmpty(fileUrl)) return "";
            var container = await CreateContainerIfNotExists();
            string relativePath = GetRelativePath(container, fileUrl);
            string filename = fileUrl.Split('/').Last();
            if (folder != "" && folder.Last() != '/') folder = folder + '/';
            string copyPath = folder == "" ? relativePath.Replace(filename, "") : folder;

            // Folowing cases:
            //      1. Duplicate image from new Scene.
            //      2. Create new exercise from template
            if (relativePath.Contains($"{_multimediaSettings.TemporalFolderName}/") || 
                folder == $"{_multimediaSettings.TemporalFolderName}/")
            {
                filename = RemoveTimestamp(filename); // removing old timestamp
                filename = DateTime.Now.ToString("ddMMyyyyHHmmssfff-") + filename; // creating new timestamp
            }
            //      3. Duplicate image from existing scene: it is not necessary to do anything

            return await MoveFile(container, relativePath, copyPath + filename, true);
        }

        public string RemoveTimestamp(string filename)
        {
            return filename.Replace(filename.Split('-').First() + "-", "");
        }

        public async Task<string> UploadFile(IFormFile file, string path = "temp/")
        {
            var container = await CreateContainerIfNotExists();
            // Slugify filename. 
            // To lowercase
            string filename = file.FileName.ToLowerInvariant();
            // Remove all accents
            filename = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(filename));
            //Replace spaces
            filename = Regex.Replace(filename, @"\s", "-", RegexOptions.Compiled);
            //Remove invalid chars
            filename = Regex.Replace(filename, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
            //Replace double occurences of - or _
            filename = Regex.Replace(filename, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            //Filename will have a timestamp
            filename = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "-" + filename;
            // get a block blob and set its type
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path + filename);
            blockBlob.Properties.ContentType = file.ContentType;
            // finally, upload the file into blob storage using the block blob reference
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
        }

        private string GetRelativePath(CloudBlobContainer container, string url)
        {
            return url.Replace(container.Uri.AbsoluteUri + "/", "");
        }

        private async void DeleteFile(CloudBlobContainer container, string relativePath)
        {
            CloudBlockBlob fileBlock = container.GetBlockBlobReference(relativePath);
            try
            {
                await fileBlock.DeleteAsync();
            }
            catch (StorageException exception)
            {
                if (exception.Message.Contains("blob does not exist")) return; //File already removed
                else throw exception;
            }
        }

        private async Task<string> MoveFile(CloudBlobContainer container, string originPath, string destinationPath, bool isCopy)
        {
            try
            {
                CloudBlockBlob oldFileBlock = container.GetBlockBlobReference(originPath);
                CloudBlockBlob newFileBlock = container.GetBlockBlobReference(destinationPath);

                await newFileBlock.StartCopyAsync(oldFileBlock.Uri);
                if (!isCopy) await oldFileBlock.DeleteAsync();
                return newFileBlock.Uri.AbsoluteUri;
            }
            catch (StorageException exception)
            {
                if (exception.Message.Contains("blob does not exist")) return ""; //File is removed
                else throw exception;
            }
        }

        private CloudBlobContainer GetContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(_azureStorageSettings.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(_azureStorageSettings.Container);
        }

        private async Task<CloudBlobContainer> CreateContainerIfNotExists()
        {
            var container = GetContainer();
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }
    }
}