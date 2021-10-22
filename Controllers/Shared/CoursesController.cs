using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Api.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Shared
{
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _coursesService;
        private readonly IClaimsService _claimsService;
        private readonly IHttpContextService _httpContextService;
        private readonly IOptions<CultureLanguagesSettings> _options;

        public CoursesController(
            ICoursesService coursesService, 
            IClaimsService claimsService,
            IHttpContextService httpContextService
        )
        {
            _coursesService = coursesService;
            _claimsService = claimsService;
            _httpContextService = httpContextService;
        }

        [HttpGet]
        public async Task<IEnumerable<int>> Get()
        {
            var subject = _httpContextService.GetSubjectFromUri();
            var language = _claimsService.GetLanguageKey();

            return await _coursesService.GetAll(subject, language);
        }
    }
}