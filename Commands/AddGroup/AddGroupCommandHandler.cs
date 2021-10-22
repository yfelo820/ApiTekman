using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Databases.Schools;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Helpers;
using Api.Interfaces.Shared;
using MediatR;

namespace Api.Commands.AddGroup
{
    public class AddGroupCommandHandler : AsyncRequestHandler<AddGroupCommand>
    {
        private const int DEFAULT_LIMIT_COURSE = 1;
        private const int DEFAULT_LIMIT_SESSION = 1;
        private const bool DEFAULT_ACCESS_ALL_SESSIONS = true;
        private const bool DEFAULT_FIRST_SESSION_WITH_DIAGNOSIS = true;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolsRepositoryUow<Group> _groupsRepository;
        private readonly IGroupSharedServiceUow _groupSharedService;

        public AddGroupCommandHandler(IUnitOfWork unitOfWork, ISchoolsRepositoryUow<Group> groupsRepository, IGroupSharedServiceUow groupSharedService)
        {
            _unitOfWork = unitOfWork;
            _groupsRepository = groupsRepository;
            _groupSharedService = groupSharedService;
        }

        protected override async Task Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            ValidateGroupName(request.Name);
            await ValidateGroup(request.TkGroupId);

            var group = new Group()
            {
                Name = request.Name,
                Course = request.Course,
                LimitCourse = DEFAULT_LIMIT_COURSE,
                LimitSession = DEFAULT_LIMIT_SESSION,
                AccessAllSessions = DEFAULT_ACCESS_ALL_SESSIONS,
                FirstSessionWithDiagnosis = DEFAULT_FIRST_SESSION_WITH_DIAGNOSIS,
                SchoolId = request.SchoolId,
                LanguageKey = request.LanguageKey,
                SubjectKey = request.SubjectKey,
                TkGroupId = request.TkGroupId
            };

            await _groupSharedService.AddGroupWithUniqueKey(group);
            await _unitOfWork.SaveChanges();
        }

        private async Task ValidateGroup(Guid tkGroupId)
        {
            var groupAlreadyExists = await _groupsRepository.Any(group => group.TkGroupId == tkGroupId);

            if (groupAlreadyExists)
            {
                throw new BadRequestException($"GROUP_ALREADY_EXISTS",$"There is already a group with given id({tkGroupId})");
            }
        }

        private void ValidateGroupName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BadRequestException("INVALID_GROUP_NAME","Cannot create group with null or empty name");
            }
        }
    }
}