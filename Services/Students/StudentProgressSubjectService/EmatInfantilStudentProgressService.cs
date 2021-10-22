using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Schools;
using Api.Interfaces.Shared;

namespace Api.Services.Students.StudentProgressSubjectService
{
    public sealed class EmatInfantilStudentProgressService : StudentProgressSubjectService
    {
        public EmatInfantilStudentProgressService(ISchoolsRepositoryUow<StudentProgress> repositoryUow)
            : base(repositoryUow)
        {
            Subject = SubjectKey.EmatInfantil;
        }

        public override async Task CreateOrUpdateProgress(
            string userName,
            int course,
            string language)
        {
            var existingProgress = await GetStudentProgress(userName, language);
            if (existingProgress == null)
            {
                var newProgress = CreateStudentProgress(userName, course, language);
                await Repository.Add(newProgress);
            }
            else if (existingProgress.Course != course)
            {
                existingProgress.Course = course;
                existingProgress.Session = 1;
                await Repository.Update(existingProgress);
            }
        }
    }
}