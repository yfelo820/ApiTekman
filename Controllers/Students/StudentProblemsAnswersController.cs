using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Services.Students.StudentAnswerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [ApiController]
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class StudentProblemsAnswersController : ControllerBase
    {
        private readonly IStudentProblemsAnswerService _studentProblemsAnswerService;

        public StudentProblemsAnswersController(IStudentProblemsAnswerService studentProblemsAnswerService)
        {
            _studentProblemsAnswerService = studentProblemsAnswerService;
        }

        [HttpPost]
        public async Task Post([FromBody] StudentProblemsAnswerDTO answer)
        {
            await _studentProblemsAnswerService.AddStudentProblemsAnswer(answer.ActivityContentBlockId, answer.Course, answer.Session, answer.Stage);
            Ok();
        }
    }
}