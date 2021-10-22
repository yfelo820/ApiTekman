using Api.DTO.Shared;
using System.Threading.Tasks;

namespace Api.Interfaces.Shared
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> TeacherLogin(string code);
        Task<LoginResponseDTO> BackofficeLogin(LoginDTO login, bool isTkReports = false);
        Task<LoginResponseDTO> StudentLogin(StudentLoginDTO login);
        Task<LoginResponseDTO> StudentLogin(string code);
    }
}