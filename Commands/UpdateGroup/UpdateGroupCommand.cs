using System;
using MediatR;

namespace Api.Commands.UpdateGroup
{
    public class UpdateGroupCommand : IRequest
    {
        public Guid TkGroupId { get; }
        public string Name { get; set; }

        public UpdateGroupCommand(Guid tkGroupId, string name)
        {
            TkGroupId = tkGroupId;
            Name = name;
        }
    }
}