using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;

namespace Api.Services.Teachers
{
    public class GroupsService : IApiService<GroupDTO>
    {
        private readonly ISchoolsRepository<Group> _groups;
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly IClaimsService _claims;
        private readonly IGroupsSharedService _groupsSharedService;
        private readonly IHttpContextService _httpContextService;

        public GroupsService(
            ISchoolsRepository<Group> repository,
            ISchoolsRepository<StudentGroup> repoStudentGroup,
            IClaimsService claims,
            IGroupsSharedService groupsSharedService,
            IHttpContextService httpContextService
        )
        {
            _groups = repository;
            _studentGroups = repoStudentGroup;
            _claims = claims;
            _groupsSharedService = groupsSharedService;
            _httpContextService = httpContextService;
        }

        public async Task<List<GroupDTO>> GetAll()
        {
            var subjectKey = _httpContextService.GetSubjectFromUri();
            var schoolId = _claims.GetSchoolId();
            var languageKey = _claims.GetLanguageKey();


            var groups = await _groups.Find(g =>
                            g.SubjectKey == subjectKey
                            && g.SchoolId == schoolId
                            && g.LanguageKey == languageKey,
                    new[] { "Students" }
            );

            return groups.ConvertAll(g => new GroupDTO(g))
                .OrderBy(g => g.Name)
                .ToList();
        }

        public async Task<Group> GetSingleGroup(Guid id)
        {
            var subjectKey = _httpContextService.GetSubjectFromUri();
            var schoolId = _claims.GetSchoolId();
            var languageKey = _claims.GetLanguageKey();

            var group = await _groups.FindSingle(
                g => g.Id == id
                    && g.SchoolId == schoolId
                    && g.SubjectKey == subjectKey,
                  //  && g.LanguageKey == languageKey,
                new[] { "Students" }
            );
            if (group == null) throw new HttpException(HttpStatusCode.NotFound);
            return group;
        }

        public async Task<GroupDTO> Update(GroupDTO groupDTO)
        {
            var group = await GetSingleGroup(groupDTO.Id);

            if (await AreStudentsInAnotherGroup(group, groupDTO.StudentUserNames) || string.IsNullOrEmpty(groupDTO.Name))
                throw new HttpException(HttpStatusCode.BadRequest);
            
            group.Students = group.Students.Where(outdatedGroupStudent =>
                groupDTO.StudentUserNames.Contains(outdatedGroupStudent.UserName)).ToList();
            
            var firstAvailableAccessNumber = 0;
            var usedAccessNumbers = group.Students.Select(s => s.AccessNumber).ToList(); 
            foreach (var updatedGroupStudentUserName in groupDTO.StudentUserNames)
            {
                var alreadyExistsStudentInGroup = group.Students.Any(outdatedGroupStudent =>
                    outdatedGroupStudent.UserName == updatedGroupStudentUserName);
                
                if(!alreadyExistsStudentInGroup)
                {
                    var accessNumber = _groupsSharedService.GetAccessNumber(usedAccessNumbers, firstAvailableAccessNumber);
                    firstAvailableAccessNumber = accessNumber + 1;
                    group.Students.Add(new StudentGroup(updatedGroupStudentUserName, groupDTO.Id, accessNumber));
                    
                    usedAccessNumbers.Add(accessNumber);
                }
            }
            
            group.AccessAllSessions = groupDTO.AccessAllSessions;
            group.Course = groupDTO.Course;
            group.FirstSessionWithDiagnosis = groupDTO.FirstSessionWithDiagnosis;
            group.LimitCourse = groupDTO.LimitCourse;
            group.LimitSession = groupDTO.LimitSession;
            group.Name = groupDTO.Name;
            group.AccessAllCourses = groupDTO.AccessAllCourses;
            group.AccessFromHome = groupDTO.AccessFromHome;
            group.ParentRating = groupDTO.ParentRating;
            await _groups.Update(group);
            await _groupsSharedService.SetProgressForStudents(groupDTO.StudentUserNames, group);
            return groupDTO;
        }

        public async Task<GroupDTO> Add(GroupDTO groupDTO)
        {
            var group = new Group(groupDTO)
            {
                SubjectKey = _httpContextService.GetSubjectFromUri(),
                SchoolId = _claims.GetSchoolId(),
                LanguageKey = _claims.GetLanguageKey()
            };

            if (
                await AreStudentsInAnotherGroup(group, groupDTO.StudentUserNames)
                || string.IsNullOrEmpty(groupDTO.Name)
            ) throw new HttpException(HttpStatusCode.BadRequest);

            group.Students = groupDTO.StudentUserNames.ConvertAll<StudentGroup>(
                stu => new StudentGroup(stu, groupDTO.Id, groupDTO.StudentUserNames.IndexOf(stu))
            );
            group = await _groupsSharedService.AddGroupWithUniqueKey(group);
            await _groupsSharedService.SetProgressForStudents(groupDTO.StudentUserNames, group);
            groupDTO.Id = group.Id;
            groupDTO.Key = group.Key;
            return groupDTO;
        }

        public async Task<GroupDTO> Delete(Guid id)
        {
            Group group = await GetSingleGroup(id);
            await _groups.Delete(group);
            return null;
        }

        public async Task<GroupDTO> GetSingle(Guid id)
        {
            var group = await GetSingleGroup(id);
            return new GroupDTO(group);
        }

        private async Task<bool> AreStudentsInAnotherGroup(Group group, IEnumerable<string> userNames)
        {
            return await _studentGroups.Any(sg =>
                userNames.Contains(sg.UserName)
                && sg.GroupId != group.Id
                && sg.Group.SubjectKey == group.SubjectKey
            );
        }
    }
}