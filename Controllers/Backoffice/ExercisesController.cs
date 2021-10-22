using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;
using Api.Services.Backoffice;
using Api.DTO.Backoffice;

namespace Api.Controllers.Backoffice
{
	public class ExercisesController : BaseBackofficeController
    {
        private readonly ExercisesService _service;
		public ExercisesController(IApiService<Exercise> service) {
			_service = service as ExercisesService;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? activityId)
        {
            return Ok(await _service.Filter(activityId.Value));
        }

		[HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }

		[HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostSceneDTO<Exercise> exercise)
        {
            return Ok(await _service.Update(exercise.GetSceneWithItems()));
        }

		[HttpPost]
        public async Task<IActionResult> Post([FromBody] PostSceneDTO<Exercise> exercise)
        {
            return Ok(await _service.Add(exercise.GetSceneWithItems()));
        }

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }

		/*
			Updates the order of an exercise. 
			Params: <exerciseIds> the ids of the exercises, 
					ordered from order = 0 to order = exerciseIds.length.
			Return: list of exercises with the updated order 
		*/
		[HttpPut("order")]
		public async Task<IActionResult> PutOrder([FromBody] Guid[] exerciseIds) 
		{
			return Ok(await _service.UpdateOrder(exerciseIds));
		}
    }
}
