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
    public class FeedbacksController : ControllerBase
    {
        private readonly FeedbacksService _service;

		public FeedbacksController(IApiService<Feedback> service) 
        {
			_service = service as FeedbacksService;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] Guid activityId, [FromQuery] float grade)
        {
            return Ok(await _service.GetForActivity(activityId, grade));
        }	
    }
}