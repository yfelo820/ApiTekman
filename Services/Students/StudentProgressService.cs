
using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
	public class StudentProgressService : IStudentProgressService
	{
        private readonly ISchoolsRepository<StudentProgress> _studentProgress;

		public StudentProgressService (
			ISchoolsRepository<StudentProgress> studentProgress
		) {
			_studentProgress = studentProgress;
		}

		public async Task NewStudentProgress(string userName, int course, string subject, string language)
		{
			var studentProgress = new StudentProgress()
            {
                UserName = userName,
                Session = 1,
                Course = course,
                SubjectKey = subject,
                LanguageKey = language,
                DiagnosisTestState = DiagnosisTestState.NotDefined
            };

			await _studentProgress.Add(studentProgress);
		}

        public async Task<StudentProgress> Get(string username, string subject, string language)
        {
            return await _studentProgress.Query()
                .FirstOrDefaultAsync(sp => sp.UserName == username &&
                sp.SubjectKey == subject &&
                sp.LanguageKey == language);
        }

        public async Task ResetStudentProgress(string userName, int course, string subject, string language)
        {
            var studentProgress = _studentProgress.Query()
                .FirstOrDefault(sp => sp.UserName == userName && 
                sp.SubjectKey == subject && 
                sp.LanguageKey == language);

            studentProgress.Course = course;
            studentProgress.Session = 1;
            studentProgress.DiagnosisTestState = DiagnosisTestState.NotDefined;

            await _studentProgress.Update(studentProgress);
        }
    }
}