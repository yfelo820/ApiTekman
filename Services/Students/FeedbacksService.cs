using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
	public class FeedbacksService : FeedbacksBaseService, IFeedbacksService
	{
		public FeedbacksService(
			IContentRepository<Feedback> feedbacks,
			IContentRepository<Activity> activities,
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IBlobStorageService blob) 
		: base(feedbacks, activities, items, itemsDrop, blob)
		{}

		public async Task<Feedback> Get(Guid activityId, float grade)
		{
			return await GetByActivityIdAndGrade(activityId, grade);
		}
	}
}