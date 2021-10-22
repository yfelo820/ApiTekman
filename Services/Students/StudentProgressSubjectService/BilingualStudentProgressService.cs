using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Schools;
using Api.Interfaces.Shared;

namespace Api.Services.Students.StudentProgressSubjectService
{
    public sealed class BilingualStudentProgressService : StudentProgressSubjectService
    {
        public BilingualStudentProgressService(ISchoolsRepository<StudentProgress> repository)
            : base(repository)
        {
        }

        public override async Task CreateOrUpdateProgress(
            string userName,
            int course,
            string language)
        {
            var inverseLanguage = GetInverseLanguage(language);
            await CreateProgressIfNotExists(userName, course, language);
            await CreateProgressIfNotExists(userName, course, inverseLanguage);
        }
        
        private string GetInverseLanguage(string language)
        {
            return language == Culture.Es ? Culture.Cat : Culture.Es;
        }
    }
}