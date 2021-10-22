using System.Threading.Tasks;

namespace Api.Services.Students.StudentProgressSubjectService
{
    public interface IStudentProgressSubjectService
    {
        Task CreateOrUpdateProgress(
            string userName,
            int course,
            string language);
    }
}