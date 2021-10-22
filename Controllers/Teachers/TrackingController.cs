using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;

		public TrackingController(ITrackingService trackingService)
		{
			_trackingService = trackingService;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? groupId)
        {
			if (!groupId.HasValue) return BadRequest();
            return Ok(await _trackingService.GetSingle(groupId.Value));
        }

        [HttpGet("session-group")]
        public async Task<IActionResult> GetGroupSessionProgress([FromQuery] Guid groupId, [FromQuery] int session)
        {
            return Ok(await _trackingService.GetGroupSessionProgress(groupId, session));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail(string userName)
        {
            return Ok(await _trackingService.GetSingleStudentDetail(userName));
        }

        [HttpPost("progress")]
		public async Task<IActionResult> PostStudentProgress([FromBody] StudentProgress progress)
		{
			return Ok(await _trackingService.UpdateStudentProgress(progress));
		}

        [HttpGet("progress-block")]
        public async Task<IActionResult> GetGroupBlockProgress([FromQuery] Guid groupID)
        {
            return Ok(await _trackingService.GetGroupBlockProgress(groupID));           
        }

        [HttpGet("progress-block-student")]
        public async Task<IActionResult> GetStudentSuperBlockProgress([FromQuery] Guid groupID, [FromQuery] string username)
        {
            return Ok(await _trackingService.GetStudentSuperBlockProgress(groupID, username));
        }

        [HttpGet("students-list-average")]
        public async Task<IActionResult> GetStudentsListSubjectsAverage([FromQuery] Guid groupID)
        {
            return Ok(await _trackingService.GetStudentsListSubjectsAverage(groupID));
        }

        [HttpPost("student-move-session")]
        public async Task<IActionResult> SetStudentToAnotherSession([FromBody] PostStudentProgressDto filterStudent)
        {
            return Ok(await _trackingService.SetStudentToAnotherSession(filterStudent.groupID, filterStudent.userName, filterStudent.session, filterStudent.course));
        }

        [HttpPost("student-restart-diagnosis")]
        public async Task<IActionResult> ResetDiagnosisTestState([FromBody] PostStudentGroupDto filterStudent)
        {
            return Ok(await _trackingService.ResetDiagnosisTestState(filterStudent.groupID, filterStudent.userName));
        }

        [HttpGet("courses-for-student")]
        public async Task<IActionResult> GetCoursesPerStudent([FromQuery] Guid groupID, [FromQuery] string userName)
        {
            return Ok(await _trackingService.GetCoursesPerStudent(groupID, userName));
        }

        [HttpGet("student-session-progress")]
        public async Task<IActionResult> GetStudentSessionProgress([FromQuery] Guid groupID,[FromQuery] string userName,[FromQuery] int course, [FromQuery] int session)
        {
            return Ok(await _trackingService.GetStudentSessionProgress(groupID, userName,course, session));
        }

        [HttpGet("activities-of-stage")]
        public async Task<IActionResult> GetActivitiesOfStage([FromQuery] Guid groupID, [FromQuery] string userName, [FromQuery] int course, [FromQuery] int session, [FromQuery] int stage)
        {
            return Ok(await _trackingService.GetActivitiesOfStage(groupID, userName, course, session,stage));
        }
    }    
}
