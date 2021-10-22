using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.TkReports;

namespace Api.Interfaces.TkReports
{
	public interface IItineraryService
	{
		Task<List<ItineraryDTO>> GetAll(string subjectKey, string languageKey);
	}
}