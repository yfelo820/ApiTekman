using System.Threading.Tasks;
using Api.DTO.Students;

namespace Api.Services.Students.StudentAnswerService
{
    public interface IStudentAnswerService
    {
        Task<StudentAnswerDTO> AddStudentAnswer(StudentAnswerDTO studentAnswer);
    }

    public interface IEmatStudentAnswerService : IStudentAnswerService { }
    public interface IEmatInfantilStudentAnswerService : IStudentAnswerService { }
    public interface ILudiStudentAnswerService : IStudentAnswerService { }
    public interface ISuperletrasStudentAnswerService : IStudentAnswerService { }
}
