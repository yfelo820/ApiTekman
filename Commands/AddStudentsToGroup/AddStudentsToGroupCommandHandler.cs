using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.DTO.Groups;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using MediatR;

namespace Api.Commands.AddStudentsToGroup
{
    public class AddStudentsToGroupCommandHandler : AsyncRequestHandler<AddStudentsToGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolsRepositoryUow<Group> _groupsRepository;
        private readonly ISchoolsRepositoryUow<StudentGroup> _studentsRepository;
        private readonly IGroupSharedServiceUow _groupSharedServiceUow;

        public AddStudentsToGroupCommandHandler(IUnitOfWork unitOfWork, ISchoolsRepositoryUow<Group> groupsRepository,
            ISchoolsRepositoryUow<StudentGroup> studentsRepository, IGroupSharedServiceUow groupSharedServiceUow)
        {
            _unitOfWork = unitOfWork;
            _groupsRepository = groupsRepository;
            _studentsRepository = studentsRepository;
            _groupSharedServiceUow = groupSharedServiceUow;
        }

        protected override async Task Handle(AddStudentsToGroupCommand request, CancellationToken cancellationToken)
        {
            ValidateStudentsUserName(request.Students);
            var group = await GetGroup(request.TkGroupId);
            await ValidateStudentsNotAssignedToAnotherGroup(group, request.Students);

            var alreadyUsedAccessNumbers = group.Students.Select(s => s.AccessNumber).ToList();
            var studentsToAdd = new List<StudentGroup>();

            foreach (var student in request.Students)
            {
                var studentInGroup = group.Students.SingleOrDefault(s => 
                    s.TkStudentId == student.TkStudentId || s.UserName == student.UserName);
                
                if (studentInGroup != null)
                {
                    if (studentInGroup.UserName != student.UserName)
                    {
                        throw new BadRequestException("USERNAME_CHANGED",
                            "A student is already assigned to group with another user name");
                    }
                    
                    continue;
                }

                var accessNumber = _groupSharedServiceUow.GetAccessNumber(alreadyUsedAccessNumbers);
                studentsToAdd.Add(new StudentGroup(student.TkStudentId, student.UserName, group.Id, accessNumber));
                alreadyUsedAccessNumbers.Add(accessNumber);
            }

            if (studentsToAdd.Any())
            {
                await _studentsRepository.Add(studentsToAdd);
                var studentsToAddUserNames = studentsToAdd.Select(studentToAdd => studentToAdd.UserName);
                await _groupSharedServiceUow.SetProgressForStudents(studentsToAddUserNames, group);
                await _unitOfWork.SaveChanges();
            }
        }

        private async Task ValidateStudentsNotAssignedToAnotherGroup(Group group, IEnumerable<GroupManagementStudentDto> groupStudents)
        {
            var tkStudentIds = groupStudents.Select(gs => gs.TkStudentId);
            var userNames = groupStudents.Select(gs => gs.UserName);
            var studentsInAnotherGroup = await _studentsRepository.Any(student =>
                (tkStudentIds.Contains(student.TkStudentId.GetValueOrDefault()) || userNames.Contains(student.UserName)) &&
                student.GroupId != group.Id &&
                student.Group.SubjectKey == group.SubjectKey);

            if (studentsInAnotherGroup)
            {
                throw new BadRequestException("STUDENT_ALREADY_ASSIGNED", "A student is already assigned to another group");
            }
        }

        private void ValidateStudentsUserName(IEnumerable<GroupManagementStudentDto> students)
        {
            if (students.Any(student => string.IsNullOrEmpty(student.UserName)))
            {
                throw new BadRequestException("USERNAME_EMPTY", "Cannot assign student to group without an user name");
            }
        }

        private async Task<Group> GetGroup(Guid tkGroupId)
        {
            var groups = await _groupsRepository.FindSingle(group => group.TkGroupId == tkGroupId,new[] { "Students" });

            return groups
                   ?? throw new BadRequestException("INVALID_TKGROUP_ID", $"Invalid Tk Group Id({tkGroupId})");
        }
    }
}