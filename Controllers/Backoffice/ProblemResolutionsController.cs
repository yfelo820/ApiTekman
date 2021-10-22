using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Api.Services.Backoffice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Backoffice
{
    [Route("backoffice/problem-resolutions")]
    [Authorize(Policy = Role.Backoffice)]
    public class ProblemResolutionsController : ControllerBase
    {
        private readonly ProblemResolutionsService _service;
        public ProblemResolutionsController(IApiService<ProblemResolution> service) => _service = service as ProblemResolutionsService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? languageId)
        {
            return Ok(await _service.Filter(languageId));
        }
    }
}
