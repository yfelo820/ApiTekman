using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Interfaces.Teachers
{
    public interface ICoursesService
	{
        Task<List<int>> GetAll(string subject, string language);
	}
}