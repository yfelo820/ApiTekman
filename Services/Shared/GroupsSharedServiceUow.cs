using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Services.Students.StudentProgressSubjectService;

namespace Api.Services.Shared
{
    /// <summary>
    /// Group shared service intended to be used with UoW.
    /// No saveChanges is called when using this service.
    /// </summary>
    public class GroupsSharedServiceUow: GroupsSharedAbstractService, IGroupSharedServiceUow
    {
        public GroupsSharedServiceUow(
            ISchoolsRepositoryUow<Group> groupsRepository,
            IStudentProgressSubjectServiceFactory studentProgressServiceFactory) 
            : base(groupsRepository, studentProgressServiceFactory)
        {
        }
    }
}