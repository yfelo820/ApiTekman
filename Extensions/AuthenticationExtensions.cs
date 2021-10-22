using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Api.Auth;
using System.Collections.Generic;

namespace Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,
            IConfiguration configuration,
            IHostingEnvironment environment)
        {
            var ssoService = services.BuildServiceProvider().GetService<ISsoService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthenticationSchemes.APIScheme;
                options.DefaultScheme = AuthenticationSchemes.APIScheme;
            })
           .AddSchemeJwtBearer(AuthenticationSchemes.SSOScheme, options =>
            {
                options.Authority = $"{configuration["sso:Url"]}/oauth2/oidcdiscovery";
                options.RequireHttpsMetadata = !environment.IsDevelopment();
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidAudiences = ssoService.GetClientIds()
                };
            })
            .AddJwtBearer(AuthenticationSchemes.APIScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
                    )
                };
            });

            return services;
        }
    }
}
