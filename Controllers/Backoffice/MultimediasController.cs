using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Backoffice.Multimedia;
using Api.Interfaces.Backoffice;
using Api.Interfaces.Shared;
using Api.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Backoffice
{
    [ApiController]
    [Route("backoffice/[controller]")]
    [Authorize(Policy = Role.Backoffice)]
    public class MultimediasController : ControllerBase
    {
        private readonly IMultimediaService _multimediaService;
        private readonly IBlobStorageService _storageService;
        private readonly MultimediaSettings _multimediaSettings;

        public MultimediasController(IMultimediaService multimediaService,
            IBlobStorageService storageService,
            IOptions<MultimediaSettings> options)
        {
            _multimediaService = multimediaService;
            _storageService = storageService;
            _multimediaSettings = options.Value;
        }

        [HttpGet("{id}")]
        public async Task<MultimediaResponse> Get(
            [FromRoute] Guid id)
            => await _multimediaService.GetSingle(id);

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] MultimediaRequest multimedia)
        {
            var id = await _multimediaService.Add(multimedia);
            await _storageService.MoveFileByUrl(multimedia.MediaUrl, $"{_multimediaSettings.FolderName}/{id}/");

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] MultimediaRequest multimedia)
        {
            var currentMultimedia = await _multimediaService.GetSingle(id);
            var existTemporalFile = string.IsNullOrEmpty(multimedia.MediaUrl) || multimedia.MediaUrl.Contains($"{_multimediaSettings.TemporalFolderName}/");
            if (existTemporalFile)
            {
                _storageService.DeleteFileByUrl(currentMultimedia.MediaUrl);
            }

            var existNewFile = !string.IsNullOrEmpty(multimedia.MediaUrl) && multimedia.MediaUrl.Contains($"{_multimediaSettings.TemporalFolderName}/");
            if (existNewFile)
            {
                multimedia.MediaUrl = await _storageService.MoveFileByUrl(multimedia.MediaUrl, $"{_multimediaSettings.FolderName}/{id}/");
            }

            await _multimediaService.Update(id, multimedia);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id)
        {
            var multimedia = await _multimediaService.GetSingle(id);
            _storageService.DeleteFileByUrl(multimedia.MediaUrl);
            await _multimediaService.Delete(id);

            return NoContent();
        }

        [HttpGet("videos")]
        public async Task<ActionResult<IEnumerable<MultimediaListItem>>> GetVideos(
            [FromQuery] Guid? subjectId,
            [FromQuery] Guid? languageId)
        {
            return await GetFiltered(subjectId, languageId, MediaTypeRequest.Video);
        }

        [HttpGet("audios")]
        public async Task<ActionResult<IEnumerable<MultimediaListItem>>> GetAudios(
            [FromQuery] Guid? subjectId,
            [FromQuery] Guid? languageId)
        {
            return await GetFiltered(subjectId, languageId, MediaTypeRequest.Audio);
        }

        private async Task<ActionResult<IEnumerable<MultimediaListItem>>> GetFiltered(
            Guid? subjectId,
            Guid? languageId,
            MediaTypeRequest mediaType)
        {
            if (!subjectId.HasValue || !languageId.HasValue)
            {
                return BadRequest();
            }

            return await _multimediaService.Filter(subjectId.Value, languageId.Value, mediaType);
        }
    }
}