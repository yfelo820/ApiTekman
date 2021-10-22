using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _studentsService;

		public StudentsController(IStudentsService studentsService)
		{
			_studentsService = studentsService;
		}

		[HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _studentsService.GetAll());
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetSingle(string userName)
        {
            try
            {
                return Ok(await _studentsService.GetSingle(userName));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> GetStudents([FromBody]List<string> userNames)
        {
            try
            {
                //TODO: Need to create specific DTO for this use case to pass stage.
                return Ok(await _studentsService.GetMultiples(userNames));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

        }

    }
}
