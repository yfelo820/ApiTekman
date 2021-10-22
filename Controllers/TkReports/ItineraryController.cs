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
    public class ItineraryController : ControllerBase
    {
        private const string RequiredParametersErr = "Parameter required: {0} ({1}). \n" +
            "Example request: /api/tkreports/itinerary?subject=emat&language=es-ES";

        private readonly IItineraryService _service;
        public ItineraryController(IItineraryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>
        Get([FromQuery] string subject, [FromQuery] string language)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return BadRequest(string.Format(RequiredParametersErr, "subject", "emat | ludi"));
            }
            if (string.IsNullOrEmpty(language))
            {
                return BadRequest(string.Format(RequiredParametersErr, "es-MX", "ca-ES | es-ES"));
            }
            return Ok(await _service.GetAll(subject, language));
        }
    }
}