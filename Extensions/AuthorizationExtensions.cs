using System.Security.Claims;
using Api.Auth;
using Api.Auth.ApiKey;
using Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationHandler, ApiKeyRequirementHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Role.Teacher, policy =>
                {
                    policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        (c.Type == ClaimTypes.Role && c.Value == Role.Teacher) ||
                        (c.Type == CustomSSOClaim.Role && c.Value == "TEACHER")
                     ));
                    policy.AddAuthenticationSchemes(AuthenticationSchemes.SSOScheme, AuthenticationSchemes.APIScheme);
                });
                options.AddPolicy(Role.Student, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Role.Student);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(Role.Backoffice, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Role.Backoffice);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(Role.TkReports, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Role.TkReports);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(Policies.ApiKey, policy =>
                {
                    var apiKey = configuration.GetValue<string>(SettingKeys.ApiKey);
                    policy.AddRequirements(new ApiKeyRequirement(apiKey));
                });
                options.AddPolicy(Role.Parent, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Role.Parent);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(Policies.TeacherOrParent, policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            c.Type == ClaimTypes.Role && c.Value == Role.Teacher ||
                            c.Type == ClaimTypes.Role && c.Value == Role.Parent ||
                            c.Type == CustomSSOClaim.Role && c.Value == "TEACHER"));
                    policy.AddAuthenticationSchemes(AuthenticationSchemes.SSOScheme, AuthenticationSchemes.APIScheme);
                });
            });

            return services;
        }
    }
}
