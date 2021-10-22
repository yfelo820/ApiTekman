using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Parents;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Parents
{
    [Route("parents/[controller]")]
    [Authorize(Policy = Role.Parent)]
    public class ParentFeedbacksController : ControllerBase
    {
        private readonly IParentFeedbackService _parentFeedbackService;
        private readonly IStudentsService _studentsService;
        private readonly IClaimsService _claimsService;

        public ParentFeedbacksController(
            IParentFeedbackService parentFeedbackService,
            IStudentsService studentsService,
            IClaimsService claimsService
        )
        {
            _parentFeedbackService = parentFeedbackService;
            _studentsService = studentsService;
            _claimsService = claimsService;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestionSet(string userName) => Ok(await _parentFeedbackService.GetQuestionSet(userName));

        [HttpGet("answers")]
        public async Task<IActionResult> GetAnswers(string userName) => Ok(await _parentFeedbackService.GetAnswers(userName));

        [HttpPost]
        public async Task<IActionResult> CreateFeedbackAnswer([FromBody] ParentFeedbackAnswerDTO parentFeedbackAnswer)
        {
            await _parentFeedbackService.Put(parentFeedbackAnswer);
            return Ok();
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertPendingFeedback([FromBody] PendingParentFeedbackDTO pendingFeedback)
        {
            await _parentFeedbackService.UpsertPendingFeedback(pendingFeedback);
            return Ok();
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeletePendingFeedback(string userName)
        {
            await _parentFeedbackService.DeletePendingFeedback(userName);
            return Ok();
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingFeedback()
        {
            var subject = _claimsService.GetSubjectKey();
            var students = (await _studentsService.GetAllStudents(subject)).ToList();
            var userNames = students.Select(s => s.UserName).ToList() ;
            var pendingFeedbacks = await _parentFeedbackService.GetPendingFeedback(userNames);
            List<PendingParentFeedbackDTOExtended> pendingFeedbacksExtended = pendingFeedbacks.Select(pf =>
            {
                return new PendingParentFeedbackDTOExtended()
                {
                    UserName = pf.UserName,
                    RequestTime = pf.RequestTime,
                    IsRead = pf.IsRead,
                    Name = students.Single(s => s.UserName == pf.UserName).Name
                };
            }).ToList();
            return Ok(pendingFeedbacksExtended);
        }

        [HttpPost("average-valuations")]
        public async Task<IActionResult> GetAverageValuationsAndComments([FromBody] List<string> userNames) => Ok(await _parentFeedbackService.GetAverageValuationsAndComments(userNames));

    }
}