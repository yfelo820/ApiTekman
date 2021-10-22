using System;
using MediatR;

namespace Api.Commands.RemoveGroup
{
    public class RemoveGroupCommand : IRequest
    {
        public Guid TkGroupId { get; }

        public RemoveGroupCommand(Guid tkGroupId)
        {
            TkGroupId = tkGroupId;
        }
    }
}