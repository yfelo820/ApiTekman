using System;
using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Interfaces.Students
{
    public interface IGroupsService
	{
		Task AddStudentGroup(string userName, int course, string subjectKey, string languageKey);
        Task<Group> GetGroupByUsername(string username);
        Task UpdateStudentGroup(Guid groupId, int course, string userName);
    }
}