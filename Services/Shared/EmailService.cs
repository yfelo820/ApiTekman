using Api.Emails.Models;
using Api.Exceptions;
using Api.Interfaces.Shared;
using FluentEmail.Core;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Api.Services.Shared
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _smtpClient;

        public EmailService(IFluentEmail smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendConfirmationEmailAsync(string email, string name, string callbackUrl, string clientUrl)
        {
            await SendEmailWithCallbackUrlAsync(
                email,
                new RegisterConfirmation()
                {
                    Name = name,
                    CallbackUrl = callbackUrl,
                    ImagesUrl = clientUrl + "/assets/img/emails/",
                    FontsUrl = clientUrl + "/assets/fonts/",
                },
                "Confirmación de registro",
                "Api.Emails.Templates.RegisterConfirmation.cshtml");
        }

        public async Task SendRemindPasswordEmailAsync(string email, string callbackUrl, string clientUrl)
        {
            await SendEmailWithCallbackUrlAsync(
                email,
                 new ResetPassword()
                 {
                     CallbackUrl = callbackUrl,
                     ImagesUrl = clientUrl + "/assets/img/emails/",
                     FontsUrl = clientUrl + "/assets/fonts/",
                 },
                "Restablecer contraseña",
                "Api.Emails.Templates.ResetPassword.cshtml");
        }

        public async Task SendWelcomeEmailAsync(string email, string clientUrl)
        {
            await SendEmailWithCallbackUrlAsync(
               email,
                new Welcome()
                {
                    CallbackUrl = clientUrl + "/login/sign_in",
                    ImagesUrl = clientUrl + "/assets/img/emails/",
                    FontsUrl = clientUrl + "/assets/fonts/",
                },
               "Bienvenido/a",
               "Api.Emails.Templates.Welcome.cshtml");
        }

        private async Task SendEmailWithCallbackUrlAsync(
            string email,
            BaseEmailModel model,
            string subject,
            string view
        )
        {
            try
            {
                await _smtpClient
                    .To(email)
                    .Subject(subject) //TODO: multilanguage
                    .UsingTemplateFromEmbedded(
                        view,
                        model,
                        GetType().GetTypeInfo().Assembly)
                    .SendAsync();

            }
            catch (Exception ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
        }
    }
}
