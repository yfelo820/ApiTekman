using System;
using System.IO;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class GroupsController : ControllerBase
    {
        private readonly IApiService<GroupDTO> _groupService;
        private readonly IStudentInfoExport _studentInfoExport;
        private readonly IStudentResultsExport _studentResultsExport;

        public GroupsController(IApiService<GroupDTO> groupService, 
            IStudentInfoExport studentInfoExport,
            IStudentResultsExport studentResultsExport)
        {
            _groupService = groupService;
            _studentInfoExport = studentInfoExport;
            _studentResultsExport = studentResultsExport;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _groupService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            try
            {
                var result = await _groupService.GetSingle(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            
        }

        [HttpGet("{id}/students-info-export")]
        public async Task<IActionResult> ExportStudents(Guid id)
        {
            var bytes = await _studentInfoExport.Export(id);
            var stream = new MemoryStream(bytes);
            return File(stream, "application/octet-stream", "StudentsList.xlsx");
        }


        [HttpGet("{id}/students-results-export")]
        public async Task<IActionResult> ExportStudentsResults(Guid id)
        {
            var bytes = await _studentResultsExport.Export(id);
            var stream = new MemoryStream(bytes);
            return File(stream, "application/octet-stream", "StudentsResults.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupDTO group)
        {   
            return Ok(await _groupService.Add(group));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] GroupDTO group)
        {
            return Ok(await _groupService.Update(group));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _groupService.Delete(id);
            return Ok(new { message = "Ok" });
        }

        [HttpGet("{id}/log-out-group")]
        public async Task<IActionResult> LogOut(Guid id)
        {
            var groupInfo = await _groupService.GetSingle(id);
            foreach(var userName in groupInfo.StudentUserNames)
            {
                //TODO Raona: Perform logout for each student if possible
            }

            return Ok(new { message = "Ok" });
        }
    }
}
