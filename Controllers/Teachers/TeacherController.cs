using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService) => _teacherService = teacherService;
         
        [HttpPost]
        public async Task<IActionResult> AddTeacher()
        {
            return Ok(await _teacherService.AddTeacher());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeacherOfSchool()
        {
            return Ok(await _teacherService.GetAllTeacherOfSchool());
        }  
    }
}
