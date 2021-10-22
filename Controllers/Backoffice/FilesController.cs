using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Backoffice
{
    public class FilesController : BaseBackofficeController
    {
        private readonly IBlobStorageService _blob;
        private readonly SupportedMediaTypes _allSupportedMediaContentTypes;

        public FilesController(IBlobStorageService blob, IOptions<SupportedMediaTypes> supportedMediaContentTypes)
        {
            _blob = blob;
            _allSupportedMediaContentTypes = supportedMediaContentTypes.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var contentTypesMaxSize = _allSupportedMediaContentTypes.ContentTypesMaxSize();
            if (contentTypesMaxSize.ContainsKey(file.ContentType) && file.Length <= contentTypesMaxSize[file.ContentType])
            {
                string url = await _blob.UploadFile(file);
                return Ok(new { url = url });
            }

            return BadRequest();

        }

        [HttpGet]
        [Route("copy")]
        public async Task<IActionResult> Copy([FromQuery] string url)
        {
            return Ok(new { url = await _blob.CopyFileByUrl(url) });
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] string url)
        {
            _blob.DeleteFileByUrl(url);
            return Ok(new { url = url });
        }
    }
}
