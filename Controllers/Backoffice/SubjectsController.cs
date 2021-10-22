using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Api.Constants;
using Api.DTO.Backoffice.Subject;
using Api.Services.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.Backoffice
{
    [ApiController]
    [Route("backoffice/[controller]")]
    [Authorize(Policy = Role.Backoffice)]
    public class SubjectsController : ControllerBase
    {   
        private readonly ISubjectsService _service;

		public SubjectsController(ISubjectsService service) => _service = service;

		[HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubjectRequest subject)
        {
            return Ok(await _service.Add(subject));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SubjectRequest subject)
        {
            return Ok(await _service.Update(id, subject));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
