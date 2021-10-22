using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.TkReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TkReports
{
    [Route("tkreports/[controller]")]
    [ApiController]
    [Authorize(Policy = Role.TkReports)]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupService;
        private const string RequiredParametersErr = "Parameter required: {0} ({1}). \n" +
            "Example request: /api/tkreports/groups?schoolId=1234&subject=emat&language=es-ES";

        public GroupsController(IGroupsService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string schoolId, 
            [FromQuery] string subject, 
            [FromQuery] string language)
        {
            if (string.IsNullOrEmpty(schoolId))
            {
                return BadRequest(string.Format(RequiredParametersErr, "schoolId", "string"));
            }
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest(string.Format(RequiredParametersErr, "subject", "emat | ludi"));
            }
            if (string.IsNullOrEmpty(language))
            {
                return BadRequest(string.Format(RequiredParametersErr, "language", "es-MX | ca-ES | es-ES"));
            }
            return Ok(await _groupService.GetAll(schoolId, subject, language));
        }
    }
}