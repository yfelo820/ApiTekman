using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Api.Constants;
using Api.DTO.Shared;
using Api.Exceptions;
using Api.Identity.Models;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.Shared
{
    public class UniversalStudentAuthService : IUniversalStudentAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGroupsService _groupsService;
        private readonly IStudentProgressService _studentProgressService;
        private readonly IEmailService _emailService;
        private readonly IRecaptchaService _recaptchaService;

        public UniversalStudentAuthService(
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGroupsService groupsService,
            IStudentProgressService studentProgressService,
            IEmailService emailService,
            IRecaptchaService recaptchaService
        )
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _groupsService = groupsService;
            _studentProgressService = studentProgressService;
            _emailService = emailService;
            _recaptchaService = recaptchaService;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO login)
        {
            var user = await AuthenticateStudentUser(login);

            var languages = new string[] { UniversalStudent.LanguageKey };
            var claims = new Claim[] {
                new Claim(CustomApiClaim.Subject, UniversalStudent.SubjectKey),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(CustomApiClaim.SchoolId, UniversalStudent.SchoolId),
                new Claim(ClaimTypes.Role, Role.Student),
                new Claim(CustomApiClaim.Languages, string.Join(",", languages))
            };

            return new LoginResponseDTO()
            {
                Token = BuildToken(claims),
                Role = Role.Student,
                Languages = languages,
                UserName = user.UserName,
                SchoolId = UniversalStudent.SchoolId,
                SchoolName = UniversalStudent.SchoolName
            };
        }

        public async Task SignUp(SignUpDto signUp)
        {
            await _recaptchaService.Validate(signUp.RecaptchaToken);

            var response = await _userManager.CreateAsync(
                new ApplicationUser()
                {
                    Email = signUp.Email,
                    UserName = signUp.Email,
                    UniversalUserProperties = new UniversalUserProperties
                    {
                        Name = signUp.Name,
                        AcceptNewsletters = signUp.AcceptNewsletters,
                        SchoolName = signUp.SchoolName,
                        SchoolCity = signUp.SchoolCity,
                        ProfileType = signUp.ProfileType
                    }
                },
                signUp.Password
            );

            if (response.Succeeded)
            {
                var user = _userManager.Users.Single(u => u.Email == signUp.Email);
                await _userManager.AddToRoleAsync(user, Role.Student);
                await _groupsService.AddStudentGroup(user.UserName,
                    signUp.Course, UniversalStudent.SubjectKey, UniversalStudent.LanguageKey);
                await _studentProgressService.NewStudentProgress(user.UserName,
                    signUp.Course, UniversalStudent.SubjectKey, UniversalStudent.LanguageKey);
                await SendConfirmationEmailAsync(user);
            }
            else if (!response.Succeeded && response.Errors.First().Code == "DuplicateUserName")
            {
                var existingUser = await _userManager.FindByEmailAsync(signUp.Email);
                if (existingUser.EmailConfirmed)
                {
                    throw new BadRequestException(BadRequestCode.UserAlreadyExist, $"The user with email: {signUp.Email} already exist.");
                }

                await SendConfirmationEmailAsync(existingUser);
            }
            else
            {
                throw new AspNetIdentityException(response.Errors.Join("\n"));
            }
        }

        public async Task SignUpConfirmation(SignUpConfirmationDto confirmation)
        {
            var user = await _userManager.FindByEmailAsync(confirmation.Email) ??
                throw new BadRequestException(BadRequestCode.UserNotExist, $"The user with email: {confirmation.Email} doesn't exist.");

            var result = await _userManager.ConfirmEmailAsync(user, Uri.UnescapeDataString(confirmation.Token));
            if (!result.Succeeded)
            {
                throw new AspNetIdentityException(result.Errors.Join("\n"));
            }

            await _emailService.SendWelcomeEmailAsync(user.Email, _config["Smtp:ClientUrl"]);
        }

        public async Task RemindPassword(RemindPasswordDto remind)
        {
            var user = await _userManager.FindByEmailAsync(remind.Email) ??
                throw new BadRequestException(BadRequestCode.UserNotExist, $"The user with email: {remind.Email} doesn't exist.");

            await SendReminderEmailAsync(user);
        }

        public async Task ResetPassword(ResetPasswordDto reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email) ??
                throw new BadRequestException(BadRequestCode.UserNotExist, $"The user with email: {reset.Email} doesn't exist.");

            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(reset.Token), reset.Password);

            if (!passwordChangeResult.Succeeded)
            {
                throw new BadRequestException(BadRequestCode.ResetPasswordError, passwordChangeResult.Errors.ElementAt(0).Description);
            }
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

        private async Task<ApplicationUser> AuthenticateStudentUser(LoginDTO login)
        {
            var result = await _signInManager.PasswordSignInAsync(
               login.Email,
               login.Password,
               login.RememberMe,
               lockoutOnFailure: false // To disable password failures to trigger account lockout
            );

            if (!result.Succeeded)
            {
                throw new HttpException(HttpStatusCode.Unauthorized);
            }

            return (await _userManager.GetUsersInRoleAsync(Role.Student))
                .FirstOrDefault(r => r.UserName == login.Email);
        }

        private async Task SendConfirmationEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackLink =
                $"{_config["Smtp:ClientUrl"]}/login/valid_token?email={user.Email}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendConfirmationEmailAsync(user.Email, user.UniversalUserProperties?.Name, callbackLink, _config["Smtp:ClientUrl"]);
        }

        private async Task SendReminderEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackLink =
                $"{_config["Smtp:ClientUrl"]}/login/reset_password?email={user.Email}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendRemindPasswordEmailAsync(user.Email, callbackLink, _config["Smtp:ClientUrl"]);
        }
    }
}
