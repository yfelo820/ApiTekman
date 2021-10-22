using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities.Content;

namespace Api.Services.Backoffice
{
    public interface ICoursesService
    {
        Task<Course> GetSingle(Guid id);
        Task<Course> GetSingle(int number);
        Task<List<Course>> GetAll();
        Task<Course> Add(Course entity);
        Task<Course> Update(Course entity);
        Task<Course> Delete(Guid id);
    }
}