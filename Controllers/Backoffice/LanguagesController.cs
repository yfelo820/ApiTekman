using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;
using Api.Services.Backoffice;

namespace Api.Controllers.Backoffice
{
	public class LanguagesController : BaseBackofficeController
    {
        private readonly ILanguagesService _service;
		public LanguagesController(ILanguagesService service) => _service = service;

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
        public async Task<IActionResult> Post([FromBody] Language language)
        {
            return Ok(await _service.Add(language));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Language language)
        {
            return Ok(await _service.Update(language));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
