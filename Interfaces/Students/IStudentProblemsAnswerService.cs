using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Interfaces.Students
{
    public interface IStudentProblemsAnswerService
	{
		Task AddStudentProblemsAnswer(Guid id, int course, int session, int stage);
        Task<StudentProblemsAnswer> Get(int course, int session, int stage);
        Task<List<StudentProblemsAnswer>> GetAll();
    }
}
