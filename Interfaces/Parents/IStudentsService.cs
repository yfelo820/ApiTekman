using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Parents;

namespace Api.Interfaces.Parents
{
    public interface IStudentsService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents(string subjectKey);
        Task<IEnumerable<string>> GetAllStudentLanguages(string email, string token, string subjectKey);
        Task<StudentDTO> GetStudent(string userName, string subjectKey);
    }
}
