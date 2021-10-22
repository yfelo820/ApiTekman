using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;
using Api.Services.Backoffice;
using Api.DTO.Backoffice;

namespace Api.Controllers.Backoffice
{
	public class TemplatesController : BaseBackofficeController
    {
        private readonly TemplatesService _service;
		public TemplatesController(IApiService<Template> service) {
			_service = service as TemplatesService;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? subjectId, [FromQuery] Guid? languageId)
        {
            return Ok(await _service.Filter(subjectId.Value, languageId.Value));
        }

		[HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, bool duplicateImages = false)
        {
            if (duplicateImages) return Ok(await _service.CopyFromTemplate(id));
            return Ok(await _service.GetSingle(id));
        }

		[HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostSceneDTO<Template> template)
        {
            var templateWithItems = template.GetSceneWithItems();
            return Ok(await _service.Update(templateWithItems));
        }

		[HttpPost]
        public async Task<IActionResult> Post([FromBody] PostSceneDTO<Template> template)
        {
            return Ok(await _service.Add(template.GetSceneWithItems()));
        }

		[HttpPost("fromExercise")]
        public async Task<IActionResult> PostFromExercise([FromBody] PostSceneDTO<Template> template, [FromQuery] Guid activityId)
        {
            return Ok(await _service.CopyFromExercise(template.GetSceneWithItems(), activityId));
        }

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
