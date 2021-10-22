using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.TkReports;

namespace Api.Interfaces.TkReports
{
	public interface IGroupsService
	{
		Task<List<GroupDTO>> GetAll(string schoolId, string subjectKey, string languageKey);
	}
}