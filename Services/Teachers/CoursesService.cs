using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.Interfaces.Teachers;

namespace Api.Services.Teachers
{
    public class CoursesService : ICoursesService
	{
        public Task<List<int>> GetAll(string subject, string language)
        { 
            // SW-4054 [TODO] develop this functionality from the backoffice
            var itineraryCourses = subject == SubjectKey.Emat && language == Culture.Mx ?
                Config.SubjectCourses[SubjectKey.EmatMx] : 
                Config.SubjectCourses[subject];

            var courses = new List<int>();
            for (var course = itineraryCourses.Start; course <= itineraryCourses.End; course++)
            {
                courses.Add(course);
            }

            return Task.FromResult(courses);
        }
	}
}