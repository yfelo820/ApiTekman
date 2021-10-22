using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Backoffice;
using Api.Entities.Content;
using Api.Filters;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Services.Backoffice;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [Route("students/problem-resolutions")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class ProblemResolutionsController: ControllerBase
    {
        private readonly ProblemResolutionsService _service;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IGroupsService _groupsService;

        public ProblemResolutionsController(IApiService<ProblemResolution> service,
            IMapper mapper, 
            IClaimsService claimsService,
            IGroupsService groupsService)
        {
            _service = service as ProblemResolutionsService;
            _mapper = mapper;
            _claimsService = claimsService;
            _groupsService = groupsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var studentUserName = _claimsService.GetUserName();
            var studentGroup = await _groupsService.GetGroupByUsername(studentUserName);

            var problemResolutions = await _service.Get(studentGroup.LanguageKey);
            
            return Ok(_mapper.Map<List<ProblemResolution>, List<ProblemResolutionDTO>>(problemResolutions));
        }
    }
}
