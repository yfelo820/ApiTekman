using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Api.Entities.Content;
using Api.Services.Shared;

namespace Api.Services.Backoffice
{
	public class AchievementsService: IApiService<Achievement>
	{
		private readonly IContentRepository<Achievement> _achievements;
		private readonly SceneService<Achievement> _sceneService;
		
		public AchievementsService(
			IContentRepository<Achievement> achievements, 
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IBlobStorageService blob
		){
			_achievements = achievements;
			_sceneService = new SceneService<Achievement>(_achievements, items, itemsDrop, blob);
		}
			
		public async Task<Achievement> Add(Achievement achievement)
		{
			return await _sceneService.Add(achievement);
		}

		public async Task<Achievement> Delete(Guid id)
		{
			return await _sceneService.Delete(id);
		}

		public async Task<List<Achievement>> Filter(Guid subjectId, Guid languageId)
		{
			return await _achievements.Query()
				.Where(e => e.SubjectId == subjectId && e.LanguageId == languageId)
				.OrderBy(e => e.Session)
				.ThenBy(e => e.Name)
				.ToListAsync();
		}

		public Task<List<Achievement>> GetAll()
		{
			throw new NotImplementedException();
			// return _sceneService.GetAll();
		}

		public async Task<Achievement> GetSingle(Guid id)
		{
			return await _sceneService.GetSingle(id);
		}

		public async Task<Achievement> Update(Achievement achievement)
		{
			return await _sceneService.Update(achievement);
		}
	}
}