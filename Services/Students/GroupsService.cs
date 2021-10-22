using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Databases.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public class GroupsService : IGroupsService
    {
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly ISchoolsRepository<Group> _groups;

        public GroupsService(
            ISchoolsRepository<StudentGroup> studentGroups,
            ISchoolsRepository<Group> groups)
        {
            _studentGroups = studentGroups;
            _groups = groups;
        }

        public async Task AddStudentGroup(string userName, int course, string subjectKey, string languageKey)
        {
            var group = await _groups.FindSingle(g => g.Name == "Universal" + course
                && g.SubjectKey == subjectKey
                && g.LanguageKey == languageKey);

            await _studentGroups.Add(new StudentGroup()
            {
                AccessNumber = await GetNextAccessNumber(group.Id),
                GroupId = group.Id,
                UserName = userName
            });
        }

        public async Task<Group> GetGroupByUsername(string username)
        {
            return await _groups.FindSingle(g => g.Students.Any(s => s.UserName == username));
        }

        public async Task UpdateStudentGroup(Guid groupId, int course, string userName)
        {
            var studentGroup = await _studentGroups
                .FindSingle(sg => sg.GroupId == groupId && sg.UserName == userName);
            await _studentGroups.Delete(studentGroup);

            await AddStudentGroup(
              userName,
              course,
              UniversalStudent.SubjectKey,
              UniversalStudent.LanguageKey
            );
        }

        private async Task<int> GetNextAccessNumber(Guid groupId)
        {
            var studentGroups = _studentGroups.Query()
                .Where(sg => sg.GroupId == groupId);

            return studentGroups.Any() ?
                await studentGroups.MaxAsync(sg => sg.AccessNumber) + 1 :
                0;
        }
    }
}