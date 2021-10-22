using Api.Constants;
using Api.Exceptions;

namespace Api.Services.Students.StudentAnswerService
{
    public class StudentAnswerServiceFactory : IStudentAnswerServiceFactory
    {
        private readonly IEmatStudentAnswerService _ematStudentAnswerService;
        private readonly IEmatInfantilStudentAnswerService _ematInfantilStudentAnswerService;
        private readonly ILudiStudentAnswerService _ludiStudentAnswerService;
        private readonly ISuperletrasStudentAnswerService _superletrasStudentAnswerService;

        public StudentAnswerServiceFactory(IEmatStudentAnswerService ematStudentAnswerService,
            IEmatInfantilStudentAnswerService ematInfantilStudentAnswerService,
            ILudiStudentAnswerService ludiStudentAnswerService,
            ISuperletrasStudentAnswerService superletrasStudentAnswerService)
        {
            _ematStudentAnswerService = ematStudentAnswerService;
            _ematInfantilStudentAnswerService = ematInfantilStudentAnswerService;
            _ludiStudentAnswerService = ludiStudentAnswerService;
            _superletrasStudentAnswerService = superletrasStudentAnswerService;
        }

        public IStudentAnswerService Create(string subject)
        {
            switch (subject)
            {
                case SubjectKey.Emat:
                    return _ematStudentAnswerService;
                case SubjectKey.EmatInfantil:
                    return _ematInfantilStudentAnswerService;
                case SubjectKey.Ludi:
                case SubjectKey.LudiCat:
                    return _ludiStudentAnswerService;
                case SubjectKey.Superletras: 
                case SubjectKey.SuperletrasCat:
                    return _superletrasStudentAnswerService;
                default:
                    throw new InvalidSubjectException($"The subject ({subject}) doesn't exist."); 
            }
        }
    }
}
