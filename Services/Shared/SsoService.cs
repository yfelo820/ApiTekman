using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Shared;
using Api.Helpers;
using Api.Interfaces.Shared;
using Microsoft.Extensions.Configuration;

namespace Api.Services.Shared
{
    public class SsoService : ISsoService
    {
        private TokenResponseDto _token;
        private IHttpContextService _httpContextService;
        private readonly HttpFormClient _oauth;
        private readonly IConfiguration _config;


        public SsoService(IConfiguration config, IHttpContextService httpContextService)
        {
            _config = config;
            var oauthBaseUrl = config["sso:Url"];
            _oauth = new HttpFormClient(oauthBaseUrl);
            _httpContextService = httpContextService;
        }

        public async Task<TokenResponseDto> GetToken(string code)
        {
            // Fetch tokens from sso
            _token = await _oauth.Post<TokenResponseDto>(
                "/oauth2/token",
                new TokenRequestDto(code, GetServiceProvider(), _httpContextService.GetRedirectUri())
            );

            return _token;
        }

        public async Task<TokenResponseDto> GetStudentToken(string code)
        {
            _token = await _oauth.Post<TokenResponseDto>(
                "/oauth2/token",
                new TokenRequestDto(code, GetStudentServiceProvider(), _httpContextService.GetStudentRedirectUri())
            );

            return _token;
        }

        public async Task<SsoClaimsDTO> GetClaims(TokenResponseDto token)
        {
            _oauth.SetAuth(token.AccessToken);
            return await _oauth.Get<SsoClaimsDTO>("/oauth2/userinfo");
        }

        public IEnumerable<string> GetClientIds()
        {
            var clientIds = new List<string>();
            var ssoSection = _config.GetSection("sso:Providers");
            foreach (var child in ssoSection.GetChildren())
            {
                clientIds.Add(child["Teacher:ClientId"]);
                clientIds.Add(child["Student:ClientId"]);
            }
            
            return clientIds.Where(id => !string.IsNullOrWhiteSpace(id));
        }

        public ServiceProvider GetServiceProvider()
        {
            var subject = _httpContextService.GetSubjectFromUri();
            var serviceProvider = new ServiceProvider();
            subject = GetSubjectForMultiRegionSite(subject);
            
            _config.GetSection($"sso:Providers:{subject}:Teacher").Bind(serviceProvider);

            return serviceProvider;
        }

        public ServiceProvider GetStudentServiceProvider()
        {
            var subject = _httpContextService.GetSubjectFromUri();
            var serviceProvider = new ServiceProvider();
            subject = GetSubjectForMultiRegionSite(subject);
            
            _config.GetSection($"sso:Providers:{subject}:Student").Bind(serviceProvider);

            return serviceProvider;
        }


        private string GetSubjectForMultiRegionSite(string subject)
        {
            if (subject == SubjectKey.Emat)
            {
                var region = _httpContextService.GetEmatRegionFromUri();
                if (region == Region.Mexico)
                {
                    subject = SubjectKey.EmatMx;
                }
                else if (region == Region.Catalonia)
                {
                    subject = SubjectKey.EmatCat;
                }
            }
            
            if (subject == SubjectKey.Superletras)
            {
                var region = _httpContextService.GetSupercibersRegionFromUri();
                if (region == Region.Catalonia)
                {
                    subject = SubjectKey.SuperletrasCat;
                }
            }

            return subject;
        }
    }
}