using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Teachers;

namespace Api.Interfaces.Teachers
{
	public interface IActivitiesService
	{
		Task<List<ActivityDTO>> GetAll(int course, int session);
	}
}