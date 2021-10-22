using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace Api.Extensions
{
    public static class SmtpExtensions
    {
        public static IServiceCollection RegisterEmailService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddFluentEmail(configuration["Smtp:From"])
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient()
                {
                    Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"]),
                    Host = configuration["Smtp:Host"],
                    Port = int.Parse(configuration["Smtp:Port"])
                });

            return services;
        }
    }
}
