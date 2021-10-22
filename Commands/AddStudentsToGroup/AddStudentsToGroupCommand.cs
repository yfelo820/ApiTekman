using System;
using System.Collections.Generic;
using Api.DTO.Groups;
using MediatR;

namespace Api.Commands.AddStudentsToGroup
{
    public class AddStudentsToGroupCommand : IRequest
    {
        public Guid TkGroupId { get; }
        public IEnumerable<GroupManagementStudentDto> Students { get; }

        public AddStudentsToGroupCommand(Guid tkGroupId, IEnumerable<GroupManagementStudentDto> students)
        {
            TkGroupId = tkGroupId;
            Students = students;
        }
    }
}