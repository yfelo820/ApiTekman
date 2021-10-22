using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Groups;

namespace Api.Interfaces.Groups
{
    public interface IGroupsManagementService
    {
        // Groups
        Task Add(GroupManagementDto groupManagementDto);
        Task Update(Guid tkGroupId, string groupName);
        Task Delete(Guid tkGroupId);

        // Students into a group
        Task AddStudents(Guid tkGroupId, IEnumerable<GroupManagementStudentDto> students);

        Task DeleteStudents(Guid tkGroupId, IEnumerable<Guid> students);
    }
}