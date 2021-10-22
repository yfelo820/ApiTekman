using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesService _service;

        public ActivitiesController(IActivitiesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ActivityDTO>> GetAll([FromQuery] int course=-1, [FromQuery] int session=-1 )
            => await _service.GetAll(course, session);
    }
}