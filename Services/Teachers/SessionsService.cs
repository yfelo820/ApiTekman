using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Teachers
{
    public class SessionsService : ISessionsService
	{
		private readonly IContentRepository<Subject> _subject;
        private readonly IContentRepository<Activity> _activities;
        private readonly IContentRepository<Course> _courses;
        private readonly IContentRepository<Language> _languages;
        private readonly IClaimsService _claims;

        public SessionsService(
            IContentRepository<Subject> subject, 
            IContentRepository<Activity> activities, 
            IContentRepository<Course> courses,
            IContentRepository<Language> languages,
            IClaimsService claims)
		{
			_subject = subject;
            _activities = activities;
            _courses = courses;
            _languages = languages;
            _claims = claims;
		}

		public async Task<SessionDTO> Get(string subject)
		{
			if (subject == SubjectKey.Emat ||
                subject == SubjectKey.EmatInfantil ||
                subject == SubjectKey.Superletras ||
                subject == SubjectKey.SuperletrasCat)
			{
				var count = (await _subject.FindSingle(s => s.Key == subject)).SessionCount;
                return new SessionDTO() { Count = count };
            }

            var ludiSessionsBySubject = (subject == SubjectKey.LudiCat) ? Config.LudiCatSessions : Config.LudiSessions;

            return new SessionDTO() { Count = ludiSessionsBySubject[0] };
        }

        public async Task<IEnumerable<CourseSessionsDTO>> GetByCourse(string subjectKey)
        {
            var languageKey = _claims.GetLanguageKey();
            var languageId = (await _languages.Find(b => b.Code == languageKey)).First().Id;
            var subjectId = (await _subject.Find(b => b.Key == subjectKey)).Select(b => b.Id).First();

            var courseAndSession = (await _activities.Find(b => b.SubjectId == subjectId && b.LanguageId == languageId))
                                    .GroupBy(b => b.CourseId)
                                    .Select(cs => new { Course = cs.Key, Session = cs.Select(a => a.Session).Max() }).ToList();

            var courseSession = new List<CourseSessionsDTO>();
            var courses = await _courses.GetAll();

            foreach (var cs in courseAndSession)
            {
                var course = courses.Find(b => b.Id == cs.Course).Number;
                courseSession.Add(new CourseSessionsDTO { CourseNumber = course, Count = cs.Session });
            }

            return courseSession;
        }
    }
}