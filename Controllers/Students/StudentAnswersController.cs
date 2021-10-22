using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Shared;
using Api.Services.Students.StudentAnswerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [ApiController]
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class StudentAnswersController : ControllerBase
    {
        private readonly IStudentAnswerService _studentAnswerService;

        public StudentAnswersController(IStudentAnswerServiceFactory studentAnswerServiceFactory, IClaimsService claimsService)
        {
            var subject = claimsService.GetSubjectKey();
            _studentAnswerService = studentAnswerServiceFactory.Create(subject);
        }

        [HttpPost]
        public async Task<StudentAnswerDTO> Post([FromBody] StudentAnswerDTO studentAnswer) => await _studentAnswerService.AddStudentAnswer(studentAnswer);
    }
}