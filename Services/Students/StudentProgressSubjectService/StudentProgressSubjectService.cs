using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Interfaces.Shared;

namespace Api.Services.Students.StudentProgressSubjectService
{
    public class StudentProgressSubjectService : IStudentProgressSubjectService
    {
        protected readonly ISchoolsRepository<StudentProgress> Repository;
        public string Subject { get; set; }

        public StudentProgressSubjectService(ISchoolsRepository<StudentProgress> repository)
        {
            Repository = repository;
        }

        public virtual async Task CreateOrUpdateProgress(
            string userName,
            int course,
            string language)
        {
            await CreateProgressIfNotExists(userName, course, language);
        }
        
        protected async Task CreateProgressIfNotExists(string userName, int course, string language)
        {
            var existingProgress = await GetStudentProgress(userName, language);
            if (existingProgress == null)
            {
                var newProgress = CreateStudentProgress(userName, course, language);
                await Repository.Add(newProgress);
            }
        }

        protected async Task<StudentProgress> GetStudentProgress(string userName, string language)
        {
            return await Repository.FindSingle(sp =>
                sp.UserName == userName &&
                sp.SubjectKey == Subject &&
                sp.LanguageKey == language);
        }

        protected StudentProgress CreateStudentProgress(string userName, int course, string language)
        {
            return new StudentProgress
            {
                UserName = userName,
                Session = 1,
                Course = course,
                SubjectKey = Subject,
                LanguageKey = language,
                DiagnosisTestState = DiagnosisTestState.NotDefined
            };
        }
    }
}