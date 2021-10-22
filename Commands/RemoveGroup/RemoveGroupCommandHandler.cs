using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using MediatR;

namespace Api.Commands.RemoveGroup
{
    public class RemoveGroupCommandHandler : AsyncRequestHandler<RemoveGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolsRepositoryUow<Group> _groupsRepository;

        public RemoveGroupCommandHandler(IUnitOfWork unitOfWork, ISchoolsRepositoryUow<Group> groupsRepository)
        {
            _unitOfWork = unitOfWork;
            _groupsRepository = groupsRepository;
        }

        protected override async Task Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await GetGroup(request.TkGroupId);
            
            _groupsRepository.Delete(group);
            await _unitOfWork.SaveChanges();
        }
        
        private async Task<Group> GetGroup(Guid tkGroupId)
        {
            return await _groupsRepository.FindSingle(group => group.TkGroupId == tkGroupId)
                   ?? throw new BadRequestException("INVALID_TKGROUP_ID", $"Invalid Tk Group Id({tkGroupId})");
        }
    }
}