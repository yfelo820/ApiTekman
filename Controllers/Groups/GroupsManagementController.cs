using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Commands.AddGroup;
using Api.Commands.AddStudentsToGroup;
using Api.Commands.RemoveGroup;
using Api.Commands.RemoveStudentsFromGroup;
using Api.Commands.UpdateGroup;
using Api.DTO.Groups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Groups
{
    [ApiController]
    [Authorize(Policy = "ApiKey")]
    [Route("groups")]
    public class GroupsManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupsManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupManagementDto group)
        {
            var request = new AddGroupCommand(group.TkGroupId, group.Course, group.Name, group.SubjectKey,
                group.LanguageKey, group.SchoolId);
            await _mediator.Send(request);
            return Ok();
        }
        
        [HttpPut("{tkGroupId}")]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupManagementUpdateDto group, Guid tkGroupId)
        {
            var request = new UpdateGroupCommand(tkGroupId, group.Name);
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{tkGroupId}")]
        public async Task<IActionResult> DeleteGroup(Guid tkGroupId)
        {
            var request = new RemoveGroupCommand(tkGroupId);
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("{tkGroupId}/students")]
        public async Task<IActionResult> AddStudents([FromBody] List<GroupManagementStudentDto> students, Guid tkGroupId)
        {
            var request = new AddStudentsToGroupCommand(tkGroupId, students);
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{tkGroupId}/students")]
        public async Task<IActionResult> DeleteStudents([FromBody] List<Guid> students, Guid tkGroupId)
        {
            var request = new RemoveStudentsFromGroupCommand(tkGroupId, students);
            await _mediator.Send(request);
            return Ok();
        }
    }
}