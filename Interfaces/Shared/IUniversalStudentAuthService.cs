using Api.DTO.Shared;
using System.Threading.Tasks;

namespace Api.Interfaces.Shared
{
    public interface IUniversalStudentAuthService
	{
		Task<LoginResponseDTO> Login(LoginDTO login);
		Task SignUp(SignUpDto signUp);
		Task SignUpConfirmation(SignUpConfirmationDto confirmation);
		Task RemindPassword(RemindPasswordDto remind);
		Task ResetPassword(ResetPasswordDto reset);
	}
}