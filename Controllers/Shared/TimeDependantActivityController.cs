using System;
using System.Threading.Tasks;
using Api.DTO.Shared;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Api.Services.Backoffice;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Shared
{
	[Route("[controller]")]
	public class TimeDependantActivityController : Controller
	{

		private readonly IApiService<Activity> _service;
        
		public TimeDependantActivityController(IApiService<Activity> service) {
			_service = service;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			Activity activity = await _service.GetSingle(id);
			if (activity == null) return NotFound();
            return Ok(new TimeDependantActivityDTO() { 
				IsTimeDependant = activity.IsTimeDependant,
				WordCount = activity.WordCount,
				QuestionCount = activity.QuestionCount,
				Course = activity.Course.Number
			});
		}
	}
}