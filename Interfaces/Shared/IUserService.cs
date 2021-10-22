using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Shared;
using Api.DTO.Teachers;

namespace Api.Interfaces.Shared
{
    public interface IUserService
	{
		Task<UserApiLoginDTO> LoginStudent (string username, string subjectKey);
		void Authenticate(string token);
		Task<IEnumerable<string>> GetLicensedLanguages(string subject);
        Task<IEnumerable<string>> GetLicensesLanguagesBySubject(string subject);
        Task<IEnumerable<StudentUserApiDTO>> GetAllStudents (string stage = Stage.Primary);
        Task<IEnumerable<StudentUserApiDTO>> GetAllStudentsByStage(string stage = Stage.Primary);
        Task<StudentUserApiDTO> GetSingleStudent (string username);
        Task<string> RegisterStudentImpersonation(string userName);
    }
}