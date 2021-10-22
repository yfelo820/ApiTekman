using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using System.Linq;
using Api.Entities.Content;
using Api.Interfaces.Teachers;
using Microsoft.EntityFrameworkCore;
using Api.DTO.Teachers;

namespace Api.Services.Teachers
{
	public class ActivitiesService: IActivitiesService
	{
		private readonly IContentRepository<Activity> _activities;
		private readonly IClaimsService _claims;
        private readonly IHttpContextService _httpContextService;

        public ActivitiesService(IContentRepository<Activity> activities, IClaimsService claims, IHttpContextService httpContextService)
		{
			_activities = activities;
			_claims = claims;
            _httpContextService = httpContextService;
        }

		public async Task<List<ActivityDTO>> GetAll(int course, int session)
		{
			var subjectKey = _httpContextService.GetSubjectFromUri();
			var languageKey = _claims.GetLanguageKey();
            var query = _activities
                .Query(new[] { "Course" })
                .Where(a =>
                    a.Subject.Key == subjectKey
                    && a.Language.Code == languageKey
                );
                if (course >= 0) query = query.Where(a => a.Course.Number == course);
                if (session >= 0) query = query.Where(a => a.Session == session);
            query = query.OrderBy(a => a.Course.Number)
            .ThenBy(a => a.Session)
            .ThenBy(a => a.Stage)
            .ThenByDescending(a => a.Difficulty);
				

            var activities = await query.ToListAsync();

            //TODO: with Automapper
            return activities.ConvertAll<ActivityDTO>(a => new ActivityDTO(a));
		}
	}
}