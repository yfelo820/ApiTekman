using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionsService _sessionsService;
        private readonly IHttpContextService _httpContextService;

        public SessionsController(ISessionsService sessionsService,
            IHttpContextService httpContextService)
        {
            _sessionsService = sessionsService;
            _httpContextService = httpContextService;
        }

        [HttpGet]
        public async Task<SessionDTO> Get() 
        {
            var subject = _httpContextService.GetSubjectFromUri();
            return await _sessionsService.Get(subject);
        }

        [HttpGet("bycourse")]
        public async Task<IEnumerable<CourseSessionsDTO>> GetByCourse([FromQuery] string subject)
        {
            return await _sessionsService.GetByCourse(subject);
        }
    }
}