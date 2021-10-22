using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Services.Students.StudentProgressSubjectService;

namespace Api.Services.Shared
{
    /// <summary>
    /// GroupSharedService not Uow
    /// </summary>
    public class GroupsSharedService : GroupsSharedAbstractService
    {
        public GroupsSharedService(
            ISchoolsRepository<Group> groupsRepository,
            IStudentProgressSubjectServiceFactory studentProgressServiceFactory) 
            : base(groupsRepository, studentProgressServiceFactory)
        {
        }
    }
}