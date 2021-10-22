using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Helpers;
using Api.Interfaces.Shared;
using Api.Services.Students.StudentProgressSubjectService;

namespace Api.Services.Shared
{
    public abstract class GroupsSharedAbstractService : IGroupsSharedService
    {
        private readonly ISchoolsRepository<Group> _groups;
        private readonly IStudentProgressSubjectServiceFactory _serviceFactory;
        private const int MaxUniqueKeyAttempts = 8;
        private const int GroupKeyLength = 5;

        protected GroupsSharedAbstractService(ISchoolsRepository<Group> groups,
            IStudentProgressSubjectServiceFactory serviceFactory)
        {
            _groups = groups;
            _serviceFactory = serviceFactory;
        }

        public async Task<Group> AddGroupWithUniqueKey(Group group, int attempt = 0)
        {
            group.Key = RandomString.Generate(GroupKeyLength);
            try
            {
                return await _groups.Add(group);
            }
            catch (Exception e)
            {
                ++attempt;
                if (attempt > MaxUniqueKeyAttempts) throw e;
                return await AddGroupWithUniqueKey(group, attempt);
            }
        }

        public int GetAccessNumber(List<int> alreadyExist, int defaultValue = 0)
        {
            if (alreadyExist.Any(x => x == defaultValue))
            {
                defaultValue++;
                return GetAccessNumber(alreadyExist, defaultValue);
            }
            
            return defaultValue;
        }

        public async Task SetProgressForStudents(IEnumerable<string> userNames, Group group)
        {
            var progressService = _serviceFactory.Create(group.SubjectKey);
            foreach (var userName in userNames)
            {
                await progressService.CreateOrUpdateProgress(userName, group.Course, group.LanguageKey);
            }
        }
    }
}
