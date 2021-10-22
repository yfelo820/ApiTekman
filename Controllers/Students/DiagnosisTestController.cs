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
    public class DiagnosisTestController : ControllerBase
    {
		private readonly IDiagnosisTestService _service;

		public DiagnosisTestController(IDiagnosisTestService service)
		{
			_service = service;
		}

		[HttpGet]
        public async Task<IActionResult> Get()
        {
			return Ok(await _service.Get());
		}
		
		[HttpPost]
        public async Task<IActionResult> Post([FromQuery] float grade)
        {
			await _service.Post(grade);
			return Ok(new { message = "Ok" });
		}		
	}
}