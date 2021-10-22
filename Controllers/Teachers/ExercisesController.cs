using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Api.Services.Backoffice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class ExercisesController : ControllerBase
    {
        private readonly ExercisesService _service;

		public ExercisesController(IApiService<Exercise> service)
        {
			_service = service as ExercisesService;
		}

		[HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid activityId)
        {
            return Ok(await _service.Filter(activityId));
        }

		[HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }
    }
}