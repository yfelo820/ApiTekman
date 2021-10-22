using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.Parents;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Parents
{
    [Route("parents/[controller]")]
    [Authorize(Policy = Role.Parent)]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _studentsService;
        private readonly IClaimsService _claimsService;

        public StudentsController(IStudentsService studentsService, IClaimsService claimsService)
        {
            _studentsService = studentsService;
            _claimsService = claimsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subject = _claimsService.GetSubjectKey();
            return Ok(await _studentsService.GetAllStudents(subject));
        }
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetSingle(string userName)
        {
            var subject = _claimsService.GetSubjectKey();
            return Ok(await _studentsService.GetStudent(userName, subject));
        }
    }
}
