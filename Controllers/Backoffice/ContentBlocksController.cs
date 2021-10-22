using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;
using Api.Services.Backoffice;

namespace Api.Controllers.Backoffice
{
	public class ContentBlocksController : BaseBackofficeController
    {
        private readonly ContentBlocksService _service;
        
		public ContentBlocksController(IApiService<ContentBlock> service)
        {
			_service = service as ContentBlocksService;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? languageId, [FromQuery] Guid? subjectId)
        {
			if (!languageId.HasValue || !subjectId.HasValue) return Ok( await _service.GetAll());
            else return Ok(await _service.Filter(languageId.Value, subjectId.Value));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContentBlock contentBlock)
        {
			if (!ModelState.IsValid) return BadRequest();
            return Ok(await _service.Add(contentBlock));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ContentBlock contentBlock)
        {
			if (!ModelState.IsValid) return BadRequest();
            return Ok(await _service.Update(contentBlock));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
