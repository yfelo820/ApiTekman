using System;
using Api.Constants;
using Api.Interfaces.Shared;
using Api.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Shared
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly GoogleAnalyticsSettings _googleAnalyticsSettings;
        private readonly IConfiguration _configuration;
        private readonly SupportedMediaTypes _supportedMediaContentTypes;
        private readonly IHttpContextService _httpContextService;
        private readonly ISsoService _ssoService;

        public ConfigurationsController(IOptions<GoogleAnalyticsSettings> options,
            IConfiguration configuration, IOptions<SupportedMediaTypes> supportedMediaContentTypes, IHttpContextService httpContextService, ISsoService ssoService)
        {
            _googleAnalyticsSettings = options.Value;
            _configuration = configuration;
            _httpContextService = httpContextService;
            _ssoService = ssoService;
            _supportedMediaContentTypes = supportedMediaContentTypes.Value;
        }

        [HttpGet("students")]
        public object GetStudentsConfiguration()
            => new
            {
                AmplitudeKey = _configuration["AmplitudeKey"],
                ApplicationInsightsKey = _configuration["Students:ApplicationInsights:InstrumentationKey"],
                GoogleAnalyticsKeys = _googleAnalyticsSettings
            };

        [HttpGet("students/is-universal")]
        public bool IsUniversalStudent() => bool.Parse(_configuration["IsUniversal"]);

        [HttpGet("teachers")]
        public object GetTeachersConfiguration()
            => new
            {
                AmplitudeKey = _configuration["AmplitudeKey"],
                ApplicationInsightsKey = _configuration["Teachers:ApplicationInsights:InstrumentationKey"],
                GoogleAnalyticsKeys = _googleAnalyticsSettings,
                FaqLink = GetFaqLink(_httpContextService.GetSubjectFromUri()),
                AuthConfig = new
                {
                    Authority = _configuration["sso:Url"],
                    ClientId = _ssoService.GetServiceProvider().ClientId,
                    Scope = _configuration["sso:Scopes"]
                }
            };

        private string GetFaqLink(string subject)
        {
            var link = _configuration[$"Teachers:Faq:{subject}"];

            if (string.IsNullOrEmpty(link) &&
                subject.Equals(SubjectKey.EmatMx, StringComparison.InvariantCultureIgnoreCase))
            {
                link = _configuration[$"Teachers:Faq:{SubjectKey.Emat}"];
            }

            return link;
        }

        [HttpGet("backoffice")]
        public object GetBackofficeConfiguration()
            => new
            {
                ApplicationInsightsKey = _configuration["Backoffice:ApplicationInsights:InstrumentationKey"],
                SupportedMediaContentTypes = _supportedMediaContentTypes
            };
    }
}
