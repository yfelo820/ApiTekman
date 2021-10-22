using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Auth;
using Api.Constants;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Api.Services.Shared
{
    public class ClaimsService : IClaimsService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IHttpContextService _httpContextService;

        public ClaimsService(IHttpContextAccessor context, IHttpContextService httpContextService)
        {
            _context = context;
            _httpContextService = httpContextService;
        } 

        public string GetLanguageKey()
        {
            // Even though the subject key is not in the claims object but the headers of the request, 
            // we will use this service for fetching its value since it works the same as the rest of 
            // the claims.
                return _context.HttpContext.Request.Headers[HeaderKey.Language];
        }

        public string GetSchoolId() 
            => _context.HttpContext.User.FindFirstValue(CustomApiClaim.SchoolId) ?? 
            _context.HttpContext.User.FindFirstValue(CustomSSOClaim.SchoolId);
        
        public string GetName() => _context.HttpContext.User.FindFirstValue(CustomApiClaim.Name);

        public string GetSurName() => _context.HttpContext.User.FindFirstValue(CustomApiClaim.SurName);

        public string GetSubjectKey() => _context.HttpContext.User.FindFirstValue(CustomApiClaim.Subject) ??
                                         _httpContextService.GetSubjectFromUri();

        public string GetUserName() => _context.HttpContext.User.FindFirstValue(CustomApiClaim.Email) ??
        _context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Contains(CustomApiClaim.EmailAddress))?.Value;

        public string GetAvailableLanguages() => _context.HttpContext.User.FindFirstValue(CustomApiClaim.Languages);

        public async Task<string> GetAccessToken() 
            => _context.HttpContext.User.FindFirstValue(CustomApiClaim.UserApiToken) ?? 
            await _context.HttpContext.GetTokenAsync(AuthenticationSchemes.SSOScheme, "access_token");
    }
}