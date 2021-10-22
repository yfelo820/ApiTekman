
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.TkReports;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Api.Interfaces.TkReports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Api.Services.TkReports
{
    public class ItineraryService: IItineraryService
	{
		private readonly IContentRepository<Subject> _subjects;
		private readonly IContentRepository<Course> _courses;
		private readonly IContentRepository<Activity> _activities;

		public ItineraryService(
			IContentRepository<Subject> subjects,
			IContentRepository<Course> courses,
			IContentRepository<Activity> activities)
		{
			_subjects = subjects;
			_courses = courses;
			_activities = activities;
		}

		public async Task<List<ItineraryDTO>> GetAll(string subjectKey, string languageKey)
		{
			var activities = await GetActivities(subjectKey, languageKey);
			if (subjectKey == SubjectKey.Emat) 
			{
				var courses = await _courses.Query().OrderBy(c => c.Number).ToListAsync();
				return await GetEmatItinerary(courses, activities);
			}
            
			return GetLudiItinerary(activities, subjectKey);
		}

		private async Task<List<Activity>> GetActivities(string subjectKey, string languageKey)
		{
			return await _activities
				.Query(new [] { "ContentBlock", "Course" })
				.Where(
					a => a.Subject.Key == subjectKey
					&& a.Language.Code == languageKey
					&& a.ContentBlockId.HasValue
					&& a.Difficulty == Config.MaxDifficultyEmat
				)
				.OrderBy(a => a.Session)
				.OrderBy(a => a.Stage)
				.ToListAsync();
		}

		private async Task<List<ItineraryDTO>> GetEmatItinerary(List<Course> courses, List<Activity> activities)
		{
			var stageCount = (await _subjects.FindSingle(s => s.Key == SubjectKey.Emat)).SessionCount;

			return courses.Select(course => new ItineraryDTO() {
				Course = course.Number,
				SessionCount = stageCount,
				Sessions = GetCourseSessions(activities, course.Number)
			})
			.ToList();
		}

		private List<ItineraryDTO> GetLudiItinerary(List<Activity> activities, string subjectKey)
		{
			int[] ludiSessionsBySubject = (subjectKey == SubjectKey.LudiCat) ? Config.LudiCatSessions : Config.LudiSessions;

			return ludiSessionsBySubject.Select(course =>
				{
					int indexCourse = ludiSessionsBySubject.IndexOf(course);
                    int numCourse = indexCourse + 1;
					return new ItineraryDTO()
					{
						Course = numCourse,
						SessionCount = indexCourse,
						Sessions = GetCourseSessions(activities, numCourse)
					};
				}
			)
			.ToList();
		}

		private IEnumerable<ItinerarySessionDTO> GetCourseSessions(List<Activity> activities, int course)
		{
			return activities
				.Where(a => a.Course.Number == course)
				.GroupBy(a => a.Session)
				.Select(activitiesBySession => new ItinerarySessionDTO() {
					Number = activitiesBySession.Key,
					ContentBlocks = activitiesBySession.Select(a => new ItineraryContentBlockDTO() {
						Id = a.ContentBlockId.Value,
						Name = a.ContentBlock.Name,
						Order = a.Stage
					})
				});
		}
	}
}