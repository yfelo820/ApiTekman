using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Interfaces.Shared
{
    public interface IGroupsSharedService
    {
        Task<Group> AddGroupWithUniqueKey(Group group, int attempt = 0);

        int GetAccessNumber(List<int> alreadyExist, int defaultValue = 0);

        Task SetProgressForStudents(IEnumerable<string> userNames, Group group);
    }
}
