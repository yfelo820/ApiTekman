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
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class ActivitiesController : ControllerBase
    {
        private readonly IClaimsService _claimsService;
        private readonly IGroupsService _groupsService;
        private readonly ActivitiesService _activitiesService;
        private readonly IMapper _mapper;

        public ActivitiesController(
            IClaimsService claimsService,
            IGroupsService groupsService,
            IApiService<Activity> service,
            IMapper mapper)
        {
            _claimsService = claimsService;
            _groupsService = groupsService;
            _activitiesService = service as ActivitiesService;
            _mapper = mapper;
        }

        [HttpGet("problem-resolutions/{problemResolutionId}")]
        public async Task<IActionResult> Get(Guid? problemResolutionId)
        {
            var studentUserName = _claimsService.GetUserName();
            var studentGroup = await _groupsService.GetGroupByUsername(studentUserName);

            return Ok(await _activitiesService.GetProblemResolutionActivities(studentGroup, problemResolutionId));
        }
    }
}
