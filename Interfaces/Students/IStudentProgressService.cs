using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Interfaces.Students
{
    public interface IStudentProgressService
	{
		Task NewStudentProgress(string userName, int course, string subject, string language);
        Task ResetStudentProgress(string userName, int course, string subject, string language);
        Task<StudentProgress> Get(string username, string subject, string language);

    }
}
