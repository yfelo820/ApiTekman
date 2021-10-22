using Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Backoffice
{
    [Route("backoffice/[controller]")]
	[Authorize(Policy = Role.Backoffice)]
    public class BaseBackofficeController : ControllerBase
    {
	}
}