using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using MediatR;

namespace Api.Commands.RemoveStudentsFromGroup
{
    public class RemoveStudentsFromGroupCommandHandler : AsyncRequestHandler<RemoveStudentsFromGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolsRepositoryUow<Group> _groupsRepository;
        private readonly ISchoolsRepositoryUow<StudentGroup> _studentsRepository;

        public RemoveStudentsFromGroupCommandHandler(IUnitOfWork unitOfWork,
            ISchoolsRepositoryUow<Group> groupsRepository, ISchoolsRepositoryUow<StudentGroup> studentsRepository)
        {
            _unitOfWork = unitOfWork;
            _groupsRepository = groupsRepository;
            _studentsRepository = studentsRepository;
        }

        protected override async Task Handle(RemoveStudentsFromGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await GetGroup(request.TkGroupId);

            var studentsToRemove = GetStudentsToRemove(group,request.Students);
            _studentsRepository.Delete(studentsToRemove.ToList());

            await _unitOfWork.SaveChanges();
        }

        private IEnumerable<StudentGroup> GetStudentsToRemove(Group group, IEnumerable<Guid> studentsToRemove)
        {
            return studentsToRemove.Select(studentToRemove =>
                group.Students.SingleOrDefault(studentInGroup =>
                    studentInGroup.TkStudentId == studentToRemove)
                ?? throw new BadRequestException("INVALID_TKSTUDENT_ID",
                    $"Invalid Tk student Id({studentToRemove}) to remove from group({group.TkGroupId})"));
        }

        private async Task<Group> GetGroup(Guid tkGroupId)
        {
            return await _groupsRepository.FindSingle(group => group.TkGroupId == tkGroupId, new[] { "Students" })
                   ?? throw new BadRequestException("INVALID_TKGROUP_ID", $"Invalid Tk Group Id({tkGroupId})");
        }
    }
}