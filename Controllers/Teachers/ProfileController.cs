using System.Linq;
using System.Threading.Tasks;
using Api.Auth;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Teachers
{
    [Authorize(Policy = Role.Teacher)]
    [Route("teachers/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextService _httpContextService;

        public ProfileController(
            IUserService userService,
            IHttpContextService httpContextService
        )
        {
            _userService = userService;
            _httpContextService = httpContextService;
        }

        [HttpGet]
        public async Task<IActionResult> getProfile()
        {
            var subjectkey = _httpContextService.GetSubjectFromUri();
            var languages = await _userService.GetLicensesLanguagesBySubject(subjectkey);
            if (!languages.Any())
            {
                return Forbid(AuthenticationSchemes.SSOScheme);
            }

            return Ok(new ProfileDTO { 
                Languages = languages
            });
        }
    }
}
