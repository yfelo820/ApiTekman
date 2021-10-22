
using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.Filters;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class FeedbacksController : ControllerBase
    {
		private readonly IFeedbacksService _service;

		public FeedbacksController(IFeedbacksService service)
		{
			_service = service;
		}

		[HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] Guid activityId, 
            [FromQuery] float grade)
        {
			return Ok(await _service.Get(activityId, grade));
		}
	}
}