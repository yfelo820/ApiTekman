using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class ParentFeedbacksController : ControllerBase
    {
        private readonly IParentFeedbackService _service;

        public ParentFeedbacksController(IParentFeedbackService service)
        {
            _service = service;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestionSet(string userName) => Ok(await _service.GetQuestionSet(userName));

        [HttpGet("answers")]
        public async Task<IActionResult> GetAnswers(string userName) => Ok(await _service.GetAnswers(userName));

        [HttpPost("pending")]
        public async Task<IActionResult> GetPendingFeedback([FromBody] List<string> userNames) => Ok(await _service.GetPendingFeedback(userNames));

        [HttpPost("average-valuations")]
        public async Task<IActionResult> GetAverageValuationsAndComments([FromBody] List<string> userNames) => Ok(await _service.GetAverageValuationsAndComments(userNames));

    }
}
