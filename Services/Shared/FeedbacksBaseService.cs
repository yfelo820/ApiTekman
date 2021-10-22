using Api.Entities.Content;
using Api.Exceptions;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Services.Shared
{
    public class FeedbacksBaseService
	{
		private readonly float[] starsByGrade = new float[] { 0.35f, 0.7f, 1.0f, 1.1f };
	
		private readonly IContentRepository<Activity> _activities;
		protected readonly IContentRepository<Feedback> _feedbacks;
		protected readonly SceneService<Feedback> _sceneService;
		
		public FeedbacksBaseService(
			IContentRepository<Feedback> feedbacks, 
			IContentRepository<Activity> activities,
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IBlobStorageService blob
		) {
			_feedbacks = feedbacks;
			_activities = activities;
			_sceneService = new SceneService<Feedback>(_feedbacks, items, itemsDrop, blob);
		}
			
		protected async Task<Feedback> GetByActivityIdAndGrade(Guid activityId, float grade)
		{
			var starCount = Array.FindIndex(starsByGrade, starGrade => starGrade > grade) + 1;
			var activity = await _activities.Get(activityId);
			var feedbackIds = await _feedbacks.Query()
				.Where(f => 
					f.SubjectId == activity.SubjectId 
					&& f.LanguageId == activity.LanguageId
					&& f.Score == starCount
				).Select(f => f.Id)
				.ToListAsync();
			if (feedbackIds.Count == 0) throw new HttpException(HttpStatusCode.NotFound);
			var randomIdx = (new Random()).Next(0, feedbackIds.Count);
			return await _sceneService.GetSingle(feedbackIds.ElementAt(randomIdx));
		}
	}
}