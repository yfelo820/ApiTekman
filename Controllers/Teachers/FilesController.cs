using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class FilesController : ControllerBase
    {   
        private readonly IBlobStorageService _blobStorageService;
		private readonly int _maxFileSize = 2 * 1000 * 1000; // 2Mb
        private readonly List<string> _supportedImageContentTypes = new List<string>
        {
            "image/gif", "image/jpeg", "image/pjpeg", "image/png", "image/svg+xml"
        };

		public FilesController(IBlobStorageService blobStorageService)
		{
			_blobStorageService = blobStorageService;
		}

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
			if (!_supportedImageContentTypes.Contains(file.ContentType) || file.Length >= _maxFileSize)
                return BadRequest();
            
			var url = await _blobStorageService.UploadFile(file);
            return Ok(new { url });
        }
    }
}
