using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Backoffice.Multimedia;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Backoffice;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class MultimediaController : ControllerBase
    {
        private readonly IClaimsService _claimsService;
        private readonly IGroupsService _groupsService;
        private readonly IMultimediaService _multimediasService;

        public MultimediaController(
            IClaimsService claimsService,
            IGroupsService groupsService,
            IMultimediaService multimediasService)
        {
            _claimsService = claimsService;
            _groupsService = groupsService;
            _multimediasService = multimediasService;
        }

        [HttpGet("videos")]
        public async Task<IEnumerable<MultimediaListItemResponse>> GetVideos()
            => await Get(MediaTypeRequest.Video);

        [HttpGet("audios")]
        public async Task<IEnumerable<MultimediaListItemResponse>> GetAudios()
            => await Get(MediaTypeRequest.Audio);

        private async Task<IEnumerable<MultimediaListItemResponse>> Get(MediaTypeRequest mediaType)
        {
            var studentUserName = _claimsService.GetUserName();
            var studentGroup = await _groupsService.GetGroupByUsername(studentUserName);

            return await _multimediasService.Get(studentGroup.SubjectKey, studentGroup.LanguageKey, 
                studentGroup.Course, mediaType);
        }
    }
}