using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Interfaces.Teachers
{
    public interface ITeacherGroupService
    {
        Task<List<TeacherGroup>> AddTeacherToGroup(Guid groupId, List<Guid> teacherIds);
        Task<List<Group>> GetGroupsOfTeacher();
    }
}
