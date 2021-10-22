using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Databases.Schools;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using MediatR;

namespace Api.Commands.UpdateGroup
{
    public class UpdateGroupCommandHandler : AsyncRequestHandler<UpdateGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolsRepositoryUow<Group> _groupsRepository;

        public UpdateGroupCommandHandler(IUnitOfWork unitOfWork, ISchoolsRepositoryUow<Group> groupsRepository)
        {
            _unitOfWork = unitOfWork;
            _groupsRepository = groupsRepository;
        }
        
        protected override async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            ValidateGroupName(request.Name);
            var group = await GetGroup(request.TkGroupId);

            group.Name = request.Name;
            await _groupsRepository.Update(group);

            await _unitOfWork.SaveChanges();
        }
        
        private void ValidateGroupName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BadRequestException("INVALID_GROUP_NAME","Cannot update group with null or empty name");
            }
        }

        private async Task<Group> GetGroup(Guid tkGroupId)
        {
            return await _groupsRepository.FindSingle(group => group.TkGroupId == tkGroupId)
                ?? throw new BadRequestException("INVALID_TKGROUP_ID", $"Invalid Tk Group Id({tkGroupId})");
        }
    }
}