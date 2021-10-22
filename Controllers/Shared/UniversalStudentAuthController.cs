using Api.DTO.Shared;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.Shared
{
    [AllowAnonymous]
    [ApiController]
    [Route("auth/universal-student")]
    public class UniversalStudentAuthController : ControllerBase
    {
        private readonly IUniversalStudentAuthService _universalStudentService;

        public UniversalStudentAuthController(
            IUniversalStudentAuthService universalStudentService)
        {
            _universalStudentService = universalStudentService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
            => Ok(await _universalStudentService.Login(login));

        [HttpPost]
        [Route("signup")]
        public async Task SignUp([FromBody] SignUpDto register)
            => await _universalStudentService.SignUp(register);

        [HttpPost]
        [Route("signup-confirmation")]
        public async Task SignUpConfirmation([FromBody] SignUpConfirmationDto confirmation)
            => await _universalStudentService.SignUpConfirmation(confirmation);

        [HttpPost]
        [Route("remind-password")]
        public async Task RemindPassword([FromBody] RemindPasswordDto remind)
            => await _universalStudentService.RemindPassword(remind);

        [HttpPost]
        [Route("reset-password")]
        public async Task ResetPassword([FromBody] ResetPasswordDto reset)
            => await _universalStudentService.ResetPassword(reset);
    }
}
