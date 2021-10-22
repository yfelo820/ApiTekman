using System.Net;
using Api.Exceptions;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class LanguageCheckerActionFilter : ActionFilterAttribute
    {
        private readonly IClaimsService _claimsService;

        public LanguageCheckerActionFilter(IClaimsService claimsService)
        {
            _claimsService = claimsService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var availableLanguages = _claimsService.GetAvailableLanguages();
            var language = _claimsService.GetLanguageKey();
            if (
                string.IsNullOrEmpty(language)
                || string.IsNullOrEmpty(availableLanguages)
                || !availableLanguages.Contains(language)
            ) throw new HttpException(HttpStatusCode.Unauthorized);
        }
    }
}
