using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Interfaces.Parents;
using Api.Interfaces.Shared;
using Api.Services.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Parents
{
    [Route("parents/[controller]")]
    [Authorize(Policy = Role.Parent)]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;
        private readonly IClaimsService _claimsService;

        public TrackingController(ITrackingService trackingService, IClaimsService claimsService)
        {
            _trackingService = trackingService;
            _claimsService = claimsService;
        }

		[HttpGet]
        public async Task<IActionResult> Get()
        {
            var subject = _claimsService.GetSubjectKey();
            return Ok(await _trackingService.GetMultiples(subject));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail(string userName)
        {
            var subject = _claimsService.GetSubjectKey();
            return Ok(await _trackingService.GetSingleStudentDetail(userName, subject));
        }
    }
}
