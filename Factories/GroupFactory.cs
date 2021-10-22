using System.Collections.Generic;
using Api.DTO.Groups;
using Api.Entities.Schools;
using Api.Interfaces.Groups;

namespace Api.Factories
{
    public class GroupFactory : IGroupFactory
    {
        private const int DEFAULT_LIMIT_COURSE = 1;
        private const int DEFAULT_LIMIT_SESSION = 1;
        private const bool DEFAULT_ACCESS_ALL_SESSIONS = true;
        private const bool DEFAULT_FIRST_SESSION_WITH_DIAGNOSIS = true;

        public Group Create(GroupManagementDto data)
        {
            return new Group()
            {
                Name = data.Name,
                Course = data.Course,
                Students = new List<StudentGroup>(),
                LimitCourse = DEFAULT_LIMIT_COURSE,
                LimitSession = DEFAULT_LIMIT_SESSION,
                AccessAllSessions = DEFAULT_ACCESS_ALL_SESSIONS,
                FirstSessionWithDiagnosis = DEFAULT_FIRST_SESSION_WITH_DIAGNOSIS,
                SchoolId = data.SchoolId,
                LanguageKey = data.LanguageKey,
                SubjectKey = data.SubjectKey,
                TkGroupId = data.TkGroupId
            };
        }
    }
}
