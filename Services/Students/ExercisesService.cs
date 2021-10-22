using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Services.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Students
{
	public class ExercisesService : IExercisesService
	{
		private readonly IContentRepository<Exercise> _exercises;
		private readonly SceneService<Exercise> _sceneService;
		
		public ExercisesService(
			IContentRepository<Exercise> exercises, 
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IContentRepository<Activity> activities,
			IBlobStorageService blob
		) {
			_exercises = exercises;
			_sceneService = new SceneService<Exercise>(_exercises, items, itemsDrop, blob);
		}

		public async Task<List<Guid>> GetByActivityId(Guid activityId)
		{
			return await _exercises.Query()
					.Where(e => e.ActivityId == activityId)
					.OrderBy(e => e.Order)
					.Select(e => e.Id)
					.ToListAsync();
		}

		public async Task<ExerciseDTO> GetSingle(Guid id)
		{
			return new ExerciseDTO(await _sceneService.GetSingle(id));
		}
	}
}