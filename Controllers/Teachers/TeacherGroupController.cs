using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Route("teachers/[controller]")]
    [Authorize(Policy = Role.Teacher)]
    public class TeacherGroupController : ControllerBase
    {
        private readonly ITeacherGroupService _teacherGroupService;

        public TeacherGroupController(ITeacherGroupService teacherGroupService) => _teacherGroupService = teacherGroupService;

        [HttpGet]
        public async Task<IActionResult> GetGroupsOfTeacher()
        {
            return Ok(await _teacherGroupService.GetGroupsOfTeacher());
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacherToGroup([FromQuery] Guid groupId, [FromBody] List<Guid> teacherIds)
        {
            return Ok(await _teacherGroupService.AddTeacherToGroup(groupId, teacherIds));
        }        
    }
}
