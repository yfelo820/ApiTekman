using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Entities.Content;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Api.Services.Shared;

namespace Api.Services.Backoffice
{
	public class ExercisesService: IApiService<Exercise>
	{
		private readonly IContentRepository<Exercise> _exercises;
		private readonly IContentRepository<Item> _items;
		private readonly IContentRepository<ItemDrop> _itemsDrop;
		private readonly SceneService<Exercise> _sceneService;
		
		public ExercisesService(
			IContentRepository<Exercise> exercises, 
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IBlobStorageService blob
		) {
			_exercises = exercises;
			_items = items;
			_itemsDrop = itemsDrop;
			_sceneService = new SceneService<Exercise>(_exercises, _items, _itemsDrop, blob);
		}
			
		public async Task<Exercise> Add(Exercise exercise)
		{
			_sceneService.CleanForeignKeys(exercise);
			try {
				int lastOrder = await _exercises.Query()
					.Where(e => e.ActivityId == exercise.ActivityId)
					.OrderByDescending(e => e.Order)
					.Select(e => e.Order)
					.FirstAsync();
				exercise.Order = lastOrder + 1;
			}
			catch(Exception e) {
				if (!(e is System.InvalidOperationException)) throw e;
				// the activity does not have any exercises, so this will be the first one
				exercise.Order = 0;
			}
			return await _sceneService.Add(exercise);
		}

		public async Task<Exercise> Delete(Guid id)
		{
			return await _sceneService.Delete(id);
		}

		public async Task<List<Exercise>> Filter(Guid activityId)
		{
			return await _exercises.Query()
				.Where(e => e.ActivityId == activityId)
				.OrderBy(e => e.Order)
				.ToListAsync();
		}

		public Task<List<Exercise>> GetAll()
		{
			return _sceneService.GetAll();
		}

		public async Task<Exercise> GetSingle(Guid id)
		{
			return await _sceneService.GetSingle(id);
		}

		public async Task<Exercise> Update(Exercise exercise)
		{
			return await _sceneService.Update(exercise);
		}

		public async Task<List<Exercise>> UpdateOrder(Guid[] exerciseIds)
		{
			var exercises = await _exercises.Query()
				.Where(e => exerciseIds.Contains(e.Id))
				.ToListAsync();
			int i = 0;
			foreach(Guid sceneId in exerciseIds) 
			{
				exercises.Find(e => e.Id == sceneId).Order = i;
				++i;
			}
			await _exercises.Update(exercises);
			return exercises.OrderBy(e => e.Order).ToList();
		}
	}
}