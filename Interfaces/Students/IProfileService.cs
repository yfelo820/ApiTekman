using System.Threading.Tasks;
using Api.DTO.Students;
namespace Api.Interfaces.Students
{
    public interface IProfileService
    {
        Task<StudentProfileDTO> GetProfileByEmail(string email);

        Task UpdateProfile(string userName, UpdateStudentProfileDTO studentProfile);
    }
}
