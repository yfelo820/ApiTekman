using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Api.Interfaces.Shared
{
    public interface IBlobStorageService
    {
        Uri Uri { get; }

        void DeleteFileByUrl(string url);

        Task<string> MoveFileByUrl(string fileUrl, string destination);
        Task<string> CopyFileByUrl(string fileUrl, string folder = "");
        Task<string> UploadFile(IFormFile file, string path = "temp/");

        string RemoveTimestamp(string fileName);
    }
}