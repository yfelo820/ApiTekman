
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
	public class StudentProblemsAnswerService : IStudentProblemsAnswerService
	{
        private readonly ISchoolsRepository<StudentProblemsAnswer> _studentProblemsAnswer;
        private readonly IClaimsService _claimsService;

        public StudentProblemsAnswerService (IClaimsService claimsService, ISchoolsRepository<StudentProblemsAnswer> studentProblemsAnswer)
        {
            _claimsService = claimsService;
            _studentProblemsAnswer = studentProblemsAnswer;
		}

		public async Task AddStudentProblemsAnswer(Guid id, int course, int session, int stage)
		{
            if (await Get(course, session, stage) == null)
            {
                var studentProblemsAnswer = new StudentProblemsAnswer()
                {
                    ActivityContentBlockId = id,
                    SubjectKey = _claimsService.GetSubjectKey(),
                    UserName = _claimsService.GetUserName(),
                    Course = course,
                    Session = session,
                    Stage = stage,
                    CreatedAt = DateTime.Now
                };

                await _studentProblemsAnswer.Add(studentProblemsAnswer);
            }
		}

        public async Task<StudentProblemsAnswer> Get(int course, int session, int stage)
        {
            var subjectKey = _claimsService.GetSubjectKey();
            var userName = _claimsService.GetUserName();

            return await _studentProblemsAnswer.Query().FirstOrDefaultAsync(item => item.SubjectKey == subjectKey && item.UserName == userName && item.Course == course && item.Session == session && item.Stage == stage);
        }

        public async Task<List<StudentProblemsAnswer>> GetAll()
        {
            var subjectKey = _claimsService.GetSubjectKey();
            var userName = _claimsService.GetUserName();

            return await _studentProblemsAnswer.Query().Where(e => e.SubjectKey == subjectKey && e.UserName == userName).ToListAsync();
        }
    }
}