using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Entities.Content;
using System;
using System.Linq;

namespace Api.Services.Backoffice
{
	public class CoursesService : ICoursesService
	{
		private readonly IContentRepository<Course> _courses;
		public CoursesService(IContentRepository<Course> repository) => _courses = repository;

		public async Task<List<Course>> GetAll()
		{
			return (await _courses.GetAll()).OrderBy(c => c.Number).ToList();
		}
	
		public async Task<Course> GetSingle(Guid id)
		{
			return await _courses.Get(id);
		}

        public async Task<Course> GetSingle(int number)
        {
            return await _courses.FindSingle(c => c.Number == number);
        }

        public async Task<Course> Add(Course course)
		{
			return await _courses.Add(course);
		}

		public async Task<Course> Update(Course course)
		{
			await _courses.Update(course);
			return course;
		}

		public async Task<Course> Delete(Guid id) {
			Course course = await GetSingle(id);
			await _courses.Delete(course);
			return course;
		}
    }
}