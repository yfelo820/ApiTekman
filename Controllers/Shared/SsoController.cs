using System;
using System.Net;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers.Shared
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class SsoController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _auth;
        private readonly IHostingEnvironment _env;
        private readonly ISchoolsRepository<SsoIdentity> _ssoRepository;
        private readonly ISsoService _ssoService;
        private readonly IHttpContextService _httpContextExtensions;
        private readonly IUserService _userService;

        public SsoController(IConfiguration config,
            IAuthService auth,
            IHostingEnvironment env,
            ISchoolsRepository<SsoIdentity> ssoRepository,
            ISsoService ssoService,
            IHttpContextService httpContextExtensions,
            IUserService userService)
        {
            _config = config;
            _auth = auth;
            _env = env;
            _ssoRepository = ssoRepository;
            _ssoService = ssoService;
            _httpContextExtensions = httpContextExtensions;
            _userService = userService;
        }

        [HttpGet("start")]
        public IActionResult TeacherLogin()
        {
            var ssoUrl = _config["sso:Url"];
            var ssoClientId = _ssoService.GetServiceProvider().ClientId;
            var redirectUrl = GetRedirectUri();
            var url = $"{ssoUrl}/oauth2/authorize?client_id={ssoClientId}&redirect_uri={redirectUrl}&scope=openid%20profile%20email%20phone%20address&response_type=code";

            return Ok(new { response = url });
        }

        [HttpGet("impersonate")]
        [Authorize(Policy = Policies.TeacherOrParent)]
        public async Task<IActionResult> ImpersonateStudent(string userName)
        {
            var ssoUrl = _config["sso:Url"];
            var ssoClientId = _ssoService.GetStudentServiceProvider().ClientId;
            var redirectUrl = GetStudentRedirectUri();
            var hash = await _userService.RegisterStudentImpersonation(userName);
            var url = $"{ssoUrl}/oauth2/authorize?client_id={ssoClientId}&redirect_uri={redirectUrl}&scope=openid%20profile%20email%20phone%20address&response_type=code&hash={hash}";

            return Ok(new { response = url });
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> TeacherLogin(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest();
            }

            // Create User object, hash it and send it back
            var userInfo = await _auth.TeacherLogin(code);
            return Ok(new { status = userInfo.Token == null ? 403 : 200, response = userInfo });
        }

        [HttpGet("student-userinfo")]
        public async Task<IActionResult> StudentLogin(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest();
            }
            
            var userInfo = await _auth.StudentLogin(code);
            if (userInfo.Token != null)
            {
                return Ok(new {status = userInfo.Token, Response = userInfo});
            }
            
            return StatusCode((int)HttpStatusCode.Forbidden, userInfo);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(Guid tokenId)
        {
            // Fixme: it should grab token via email
            var tokens = await _ssoRepository.FindSingle(s => s.Id == tokenId);
            if (tokens == null)
            {
                return Redirect(GetTeachersLoginUri());
            }

            await _ssoRepository.Delete(tokens);
            var ssoUrl = _config["sso:Url"];

            return Redirect(
                $"{ssoUrl}/oidc/logout?id_token_hint={tokens.IdToken}&post_logout_redirect_uri={GetRedirectUri()}&state=state_1");
        }

        [HttpGet("student-logout")]
        public async Task<IActionResult> StudentLogout(Guid tokenId)
        {
            var tokens = await _ssoRepository.FindSingle(s => s.Id == tokenId);
            if (tokens == null)
            {
                return Redirect(GetTeachersLoginUri());
            }

            await _ssoRepository.Delete(tokens);
            var ssoUrl = _config["sso:Url"];

            return Redirect(
                $"{ssoUrl}/oidc/logout?id_token_hint={tokens.IdToken}&post_logout_redirect_uri={GetStudentRedirectUri()}&state=state_1");
        }

        private string GetRedirectUri()
        {
            return  _httpContextExtensions.GetSubjectFromUri() == SubjectKey.EmatInfantil
                ? $"{GetServerUri()}/teachers-infantil/callback"
                : $"{GetServerUri()}/teachers/callback";
        }

        private string GetStudentRedirectUri()
        {
            return $"{GetServerUri()}/students/login/callback";
        }

        private string GetTeachersLoginUri()
        {
            return _httpContextExtensions.GetSubjectFromUri() == SubjectKey.EmatInfantil
                ? $"{GetServerUri()}/teachers-infantil/login"
                : $"{GetServerUri()}/teachers/login";
        }

        public string GetServerUri()
        {
            var protocol = _env.IsDevelopment() ? "http://" : "https://";
            var host = HttpContext.Request.Host.ToString().Replace("www.", "");
            return protocol + host;
        }
    }
}