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
	public class FeedbacksService: FeedbacksBaseService, IApiService<Feedback>
	{
		public FeedbacksService(
			IContentRepository<Feedback> feedbacks,
			IContentRepository<Activity> activities,
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IBlobStorageService blob) 
		: base(feedbacks, activities, items, itemsDrop, blob)
		{}
			
		public async Task<Feedback> Add(Feedback feedback)
		{
			return await _sceneService.Add(feedback);
		}

		public async Task<Feedback> Delete(Guid id)
		{
			return await _sceneService.Delete(id);
		}

		public async Task<List<Feedback>> Filter(Guid subjectId, Guid languageId)
		{
			return await _feedbacks.Query()
				.Where(e => e.SubjectId == subjectId && e.LanguageId == languageId)
				.OrderBy(e => e.Score)
				.ToListAsync();
		}

		public async Task<Feedback> GetForActivity(Guid activityId, float grade)
		{
			return await GetByActivityIdAndGrade(activityId, grade);
		}

		public Task<List<Feedback>> GetAll()
		{
			return _sceneService.GetAll();
		}

		public async Task<Feedback> GetSingle(Guid id)
		{
			return await _sceneService.GetSingle(id);
		}

		public async Task<Feedback> Update(Feedback feedback)
		{
			return await _sceneService.Update(feedback);
		}
	}
}