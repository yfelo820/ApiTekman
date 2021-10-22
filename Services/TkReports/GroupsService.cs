
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTO.TkReports;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.TkReports;

namespace Api.Services.TkReports
{
	public class GroupsService: IGroupsService
	{
		private readonly ISchoolsRepository<Group> _groups;

		public GroupsService(ISchoolsRepository<Group> groups)
		{
			_groups = groups;
		}

		public async Task<List<GroupDTO>> GetAll(string schoolId, string subjectKey, string language)
		{
			var groups = await _groups.Find(g => 
				g.SchoolId == schoolId 
				&& g.SubjectKey == subjectKey 
				&& g.LanguageKey == language,
				new [] { "Students" }
			);
			var usernames = groups.SelectMany(g => g.Students).Select(s => s.UserName).ToList();
			return groups.ConvertAll<GroupDTO>(g => {
				var groupUserNames = g.Students.Select(st => st.UserName);
				return new GroupDTO() {
					Id = g.Id,
					Name = g.Name,
					Course = g.Course,
					Students = usernames.Select(u => new StudentNameDTO() { UserName = u }).ToList()
				};
			});
		}
	}
}