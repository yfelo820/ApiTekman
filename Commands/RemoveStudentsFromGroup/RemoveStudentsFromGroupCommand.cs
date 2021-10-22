using System;
using System.Collections.Generic;
using MediatR;

namespace Api.Commands.RemoveStudentsFromGroup
{
    public class RemoveStudentsFromGroupCommand : IRequest
    {
        public Guid TkGroupId { get; }
        public IEnumerable<Guid> Students { get; }

        public RemoveStudentsFromGroupCommand(Guid tkGroupId, IEnumerable<Guid> students)
        {
            TkGroupId = tkGroupId;
            Students = students;
        }
    }
}