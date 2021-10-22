using System;
using System.Threading.Tasks;
using Api.APIsMailing.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.APIsMailing
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailController : ControllerBase
    {
        private readonly IMasterServiceDetail _masterService;

        public DetailController(IMasterServiceDetail masterService) => _masterService = masterService;

        [HttpGet]
        public async Task<IActionResult> GetGroupDetail([FromQuery] Guid groupId, [FromQuery] string username, [FromQuery] int session)
        {
            return Ok(await _masterService.GetMasterResponse(groupId, username, session));
        }
    }
}
