using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Api.Services.Backoffice;
using Api.DTO.Backoffice;
using Api.Entities.Content;
using Api.Interfaces.Shared;

namespace Api.Controllers.Backoffice
{
	public class AchievementsController : BaseBackofficeController
    {
        private readonly AchievementsService _service;
		public AchievementsController(IApiService<Achievement> service) {
			_service = service as AchievementsService;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? subjectId, [FromQuery] Guid? languageId)
        {
            return Ok(await _service.Filter(subjectId.Value, languageId.Value));
        }

		[HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }

		[HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostSceneDTO<Achievement> achievements)
        {
            return Ok(await _service.Update(achievements.GetSceneWithItems()));
        }

		[HttpPost]
        public async Task<IActionResult> Post([FromBody] PostSceneDTO<Achievement> achievements)
        {
            return Ok(await _service.Add(achievements.GetSceneWithItems()));
        }

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
