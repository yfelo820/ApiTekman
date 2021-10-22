using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Shared;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Identity.Models;
using Api.Interfaces.Parents;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.Shared
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly ISsoService _sso;
        private readonly ISchoolsRepository<SsoIdentity> _identityRepository;
        private readonly IHttpContextService _httpContextExtensions;
        private readonly IStudentsService _studentsService;

        public AuthService(
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            ISchoolsRepository<StudentGroup> studentGroups,
            ISsoService sso,
            ISchoolsRepository<SsoIdentity> identityRepository,
            IHttpContextService httpContextExtensions,
            IStudentsService studentsService)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _studentGroups = studentGroups;
            _sso = sso;
            _identityRepository = identityRepository;
            _httpContextExtensions = httpContextExtensions;
            _studentsService = studentsService;
        }


        public async Task<LoginResponseDTO> BackofficeLogin(LoginDTO login, bool isTkReports = false)
        {
            var user = await AuthenticateBackofficeUser(login) ??
                       throw new HttpException(HttpStatusCode.Unauthorized);

            var role = isTkReports ? Role.TkReports : Role.Backoffice;
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            return new LoginResponseDTO() { Token = BuildToken(claims) };
        }

        public async Task<LoginResponseDTO> TeacherLogin(string code)
        {
            var token = await _sso.GetToken(code);
            var claimsDto = await _sso.GetClaims(token);

            _userService.Authenticate(token.AccessToken);
            var subject =  _httpContextExtensions.GetSubjectFromUri();

            var userRole = new List<string>() { Role.Teacher, Role.Parent }.Find(role => {
                var selectRole = claimsDto.roles.Split(",").FirstOrDefault(item => {
                    return string.Equals(role, item, StringComparison.CurrentCultureIgnoreCase);
                });

                return string.Equals(role, selectRole, StringComparison.CurrentCultureIgnoreCase);
            });

            var ssoIdentity = await GetIdentity(claimsDto, token.IdToken);
            await SaveIdentity(ssoIdentity);

            if (userRole == null)
            {
                return new LoginResponseDTO { TokenId = ssoIdentity.Id };
            }

            string[] languages = {};
            if (userRole == Role.Teacher)
            {
                languages = (await _userService.GetLicensedLanguages(subject)).ToArray();
            }
            else
            {
                //Parent Login
                languages = (await _studentsService.GetAllStudentLanguages(claimsDto.email, token.AccessToken, subject)).ToArray();
                
                if (!languages.Any())
                {
                    // Default language
                    languages = new [] { "es-ES" };
                }
            }

            var claims = GetClaims(userRole, claimsDto, token, subject, languages);

            return new LoginResponseDTO()
            {
                Token = BuildToken(claims),
                TokenId = ssoIdentity.Id,
                Photo = claimsDto.picture,
                Role = userRole,
                Languages = languages,
                Name = claimsDto.given_name + " " + claimsDto.family_name,
                Email = claimsDto.email,
                SchoolId = claimsDto.school_id.ToString(),
                SchoolName = claimsDto.school_name
            };
        }

        public async Task<LoginResponseDTO> StudentLogin(StudentLoginDTO login)
        {
            // Checking that the student provided has a group with a correct AccessNumber, UserName and GroupKey
            var studentGroup = await _studentGroups.Query(new string[] { "Group" })
                .Where(s =>
                    s.AccessNumber == login.AccessNumber
                    && s.UserName == login.UserName
                    && s.Group.Key == login.GroupKey
                    && s.Group.SubjectKey == login.SubjectKey
                ).FirstOrDefaultAsync();
            if (studentGroup == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized);
            }

            // Login to user API with the default student password
            var user = await _userService.LoginStudent(
                studentGroup.UserName,
                login.SubjectKey
            );
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized);
            }

            // Ciberludicat or SuperletrasCat can be used in Catalan and Spanish regardless of the language of the licenses.
            var languages = login.SubjectKey == SubjectKey.LudiCat || login.SubjectKey == SubjectKey.SuperletrasCat
                ? new[] { Culture.Es, Culture.Cat }
                : new[] { studentGroup.Group.LanguageKey };

            var claims = GetStudentClaims(user, login.SubjectKey, languages);
            return new LoginResponseDTO()
            {
                Token = BuildToken(claims),
                Photo = user.Photo,
                Role = Role.Student,
                Languages = languages,
                GroupKey = studentGroup.Group.Key,
                Name = user.Name,
                UserName = user.UserName,
                SchoolId = user.SchoolId,
                SchoolName = user.SchoolName
            };
        }

        public async Task<LoginResponseDTO> StudentLogin(string code)
        {
            var token = await _sso.GetStudentToken(code);
            var claimsDto = await _sso.GetClaims(token);

            _userService.Authenticate(token.AccessToken);
            var subject =  _httpContextExtensions.GetSubjectFromUri();

            var hasStudentRole = claimsDto.roles
                .Split(",")
                .Any(item =>
                    string.Equals(item, Role.Student, StringComparison.CurrentCultureIgnoreCase));
            
            if (!hasStudentRole)
            {
                throw new HttpException(HttpStatusCode.Unauthorized,
                    "User does not have a Student role.");
            }
            
            // Checking that the student provided has a group with a correct subject key
            var studentGroup = await _studentGroups.Query(new string[] { "Group" })
                .Where(s =>
                    s.UserName == claimsDto.email
                    && s.Group.SubjectKey == subject
                ).SingleOrDefaultAsync();
            
            if (studentGroup == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized,
                    $"Student is not member of a group for subject {subject}");
            }
            
            var ssoIdentity = await GetIdentity(claimsDto, token.IdToken);
            await SaveIdentity(ssoIdentity);

            var languages = subject == SubjectKey.LudiCat
                ? new[] { Culture.Es, Culture.Cat }
                : new[] { studentGroup.Group.LanguageKey };

            var claims = GetClaims(Role.Student, claimsDto, token, subject, languages);

            return new LoginResponseDTO()
            {
                Token = BuildToken(claims),
                TokenId = ssoIdentity.Id,
                Photo = claimsDto.picture,
                Role = Role.Student,
                Languages = languages,
                GroupKey = studentGroup.Group.Key,
                Name = claimsDto.given_name + " " + claimsDto.family_name,
                Email = claimsDto.email,
                UserName = claimsDto.email, //username is email when infantil
                SchoolId = claimsDto.school_id.ToString(),
                SchoolName = claimsDto.school_name
            };
        }

        private Claim[] GetStudentClaims(UserApiLoginDTO user, string subject, string[] languages)
        {
            return new[]
            {
                new Claim(CustomApiClaim.UserApiToken, user.Token),
                new Claim(CustomApiClaim.Subject, subject),
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(CustomApiClaim.SchoolId, user.SchoolId),
                new Claim(ClaimTypes.Role, Role.Student),
                new Claim(CustomApiClaim.Languages, string.Join(",", languages))
            };
        }

        private Claim[] GetClaims(string role, SsoClaimsDTO ssoClaims, TokenResponseDto tokens, string subjectKey, string[] languages)
        {
            return new[]
            {
                new Claim(CustomApiClaim.UserApiToken, tokens.AccessToken),
                new Claim(CustomApiClaim.Subject, subjectKey),
                new Claim(ClaimTypes.Email, ssoClaims.email),
                new Claim(CustomApiClaim.SchoolId, ssoClaims.school_id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(CustomApiClaim.Languages, string.Join(",", languages))
            };
        }

        private string BuildToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_config["Jwt:ExpirationDays"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ApplicationUser> AuthenticateBackofficeUser(LoginDTO login)
        {
            var result = await _signInManager.PasswordSignInAsync(
                login.Email,
                login.Password,
                login.RememberMe,
                lockoutOnFailure: false // To disable password failures to trigger account lockout
            );
            if (!result.Succeeded)
            {
                return null;
            }

            return await _userManager.Users.FirstOrDefaultAsync(r => r.UserName == login.Email);
        }

        private async Task<SsoIdentity> GetIdentity(SsoClaimsDTO ssoClaims, string idToken)
        {
            // Save id token to use it later for logout
            var identity = (await _identityRepository.FindSingle(s => s.Email == ssoClaims.email))
                           ?? new SsoIdentity() { Email = ssoClaims.email };

            identity.IdToken = idToken;

            return identity;
        }

        private async Task SaveIdentity(SsoIdentity identity)
        {
            if (identity.Id != Guid.Empty)
            {
                await _identityRepository.Update(identity);
            }
            else
            {
                await _identityRepository.Add(identity);
            }
        }
    }
}