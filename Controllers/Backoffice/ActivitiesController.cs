using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;
using Api.Services.Backoffice;
using Api.DTO.Backoffice;
using AutoMapper;
using System.Collections.Generic;

namespace Api.Controllers.Backoffice
{
	public class ActivitiesController : BaseBackofficeController
    {
        private readonly ActivitiesService _service;
        private readonly IMapper _mapper;
        
		public ActivitiesController(IApiService<Activity> service, IMapper mapper) {
			_service = service as ActivitiesService;
            _mapper = mapper;
		}

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Activity activity = await _service.GetSingle(id);
            return Ok(_mapper.Map<Activity, ActivityDTO>(activity));
        }

        [HttpGet("{id}/isDifficultyValid")]
        public async Task<IActionResult> GetIsDifficultyValid(Guid id)
        {
            return Ok(await _service.IsDifficultyValid(id));
        }

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? subjectId, [FromQuery] Guid? languageId, [FromQuery] Guid? courseId = null, Guid? problemResolutionId = null)
        {
            if (!subjectId.HasValue)
            {
                return BadRequest();
            }

            var activities = await _service.Filter(subjectId, languageId, courseId, problemResolutionId);

            return Ok(_mapper.Map<List<Activity>, List<ActivityDTO>>(activities));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivityDTO activityDTO)
        {
            Activity activity = _mapper.Map<ActivityDTO, Activity>(activityDTO);
            if (!ModelState.IsValid) return BadRequest();
            Activity result = await _service.Add(activity);
            return Ok(_mapper.Map<Activity,ActivityDTO>(result));
        }

        [HttpPost("copyFromActivity/{subjectId}/{languageId}")]
        public async Task<IActionResult> CopyFromActivity(
			Guid subjectId,
			Guid languageId,
			[FromBody] List<CopyActivityDTO> activitiesDTO
		){
            await _service.CopyFromActivity(activitiesDTO, subjectId, languageId);
            return Ok(new { message = "ok" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ActivityDTO activityDTO)
        {
            Activity activity = _mapper.Map<ActivityDTO, Activity>(activityDTO);
			if (!ModelState.IsValid) return BadRequest();
            Activity result = await _service.Update(activity);
            return Ok(_mapper.Map<Activity,ActivityDTO>(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
