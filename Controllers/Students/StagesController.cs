using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Shared;
using Api.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [Route("students/courses/{course}/sessions/{session}/stages")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    [ApiController]
    public class StagesController : ControllerBase
    {
        private readonly IStageService _stagesService;

        public StagesController(IStageServiceFactory stageServiceFactory,
            IClaimsService claimsService)
        {
            var subject = claimsService.GetSubjectKey();
            _stagesService = stageServiceFactory.Create(subject);
        }

        [HttpGet("{stage}/next")]
        public async Task<StageActivityDTO> GetNext(
            [FromRoute] int stage,
            [FromRoute] int course,
            [FromRoute] int session
            )
            => await _stagesService.GetNext(course, session, stage);

        [HttpGet]
        public async Task<IEnumerable<StageActivityDTO>> GetStages(
            [FromRoute] int course,
            [FromRoute] int session)
            => await _stagesService.GetAll(course, session);
    }
}