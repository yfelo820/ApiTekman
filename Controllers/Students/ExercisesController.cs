using System;
using System.Text;
using System.Threading.Tasks;
using Api.Constants;
using Api.Filters;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Controllers.Students
{
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class ExercisesController : ControllerBase
    {
		private readonly IExercisesService _service;

		public ExercisesController(IExercisesService service)
		{
			_service = service;
		}

		[HttpGet]
        public async Task<IActionResult> GetByActivityId(
            [FromQuery] Guid activityId)
        {
			if (activityId == null) return BadRequest();
			return Ok(await _service.GetByActivityId(activityId));
		}

		[HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
			// We base64 the response in order to hide the solutions of the exercise from the final 
			// user in the developer console.
			var response = JsonConvert.SerializeObject(
				await _service.GetSingle(id), 
				new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver()}
			);
    		var responseBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));
            return Ok(new { response = responseBase64 });
        }
	}
}