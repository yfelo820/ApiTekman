using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Shared;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Shared
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("backofficeLogin")]
        public async Task<IActionResult> BackofficeLogin([FromBody] LoginDTO login)
        {
            if (login.Email == Config.DefaultTkReportsUser)
            {
                return BadRequest();
            }

            return Ok(await _service.BackofficeLogin(login));
        }

        [HttpPost]
        [Route("tkreportsLogin")]
        public async Task<IActionResult> TkReportsLogin([FromBody] LoginDTO login)
        {
            var loginResponse = await _service.BackofficeLogin(login, isTkReports: true);
            return Ok(new { Token = loginResponse.Token });
        }

        [HttpPost]
        [Route("student/login")]
        public async Task<IActionResult> StudentLogin([FromBody] StudentLoginDTO login)
            => Ok(await _service.StudentLogin(login));
    }
}
