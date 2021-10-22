using System;
using System.Threading.Tasks;
using Api.APIsMailing.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.APIsMailing
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalController : ControllerBase
    {
        private readonly IMasterServiceGlobal _masterService;

        public GlobalController(IMasterServiceGlobal masterService) => _masterService = masterService;

        [HttpGet]
        public async Task<IActionResult> GetGroupDetail([FromQuery] Guid groupId)
        {
            return Ok(await _masterService.GetMasterResponse(groupId));
        }

        [HttpGet("ematid")]
        public async Task<IActionResult> GetEmatGroupsIds()
        {
            return Ok(await _masterService.GetAllEmatGroupDetails());
        }
    }
}
