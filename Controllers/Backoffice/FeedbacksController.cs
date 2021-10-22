using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;
using Api.Services.Backoffice;
using Api.DTO.Backoffice;

namespace Api.Controllers.Backoffice
{
	public class FeedbacksController : BaseBackofficeController
    {
        private readonly FeedbacksService _service;
		public FeedbacksController(IApiService<Feedback> service) {
			_service = service as FeedbacksService;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid subjectId, [FromQuery] Guid languageId)
        {
            return Ok(await _service.Filter(subjectId, languageId));
        }

		[HttpGet("activity")]
        public async Task<IActionResult> Get([FromQuery] Guid activityId, [FromQuery] float grade)
        {
            return Ok(await _service.GetForActivity(activityId, grade));
        }

		[HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }

		[HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostSceneDTO<Feedback> feedback)
        {
            if (!ModelState.IsValid) return BadRequest();
            return Ok(await _service.Update(feedback.GetSceneWithItems()));
        }

		[HttpPost]
        public async Task<IActionResult> Post([FromBody] PostSceneDTO<Feedback> feedback)
        {
            if (!ModelState.IsValid) return BadRequest();
            return Ok(await _service.Add(feedback.GetSceneWithItems()));
        }

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
