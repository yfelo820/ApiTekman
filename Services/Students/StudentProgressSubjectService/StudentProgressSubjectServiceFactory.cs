using System;
using Api.Constants;
using Api.Exceptions;

namespace Api.Services.Students.StudentProgressSubjectService
{
    public class StudentProgressSubjectServiceFactory : IStudentProgressSubjectServiceFactory
    {
        private readonly StudentProgressSubjectService _commonService;
        private readonly EmatInfantilStudentProgressService _ematInfantilService;
        private readonly BilingualStudentProgressService _bilingualService;

        public StudentProgressSubjectServiceFactory(
            StudentProgressSubjectService commonService,
            EmatInfantilStudentProgressService ematInfantilService,
            BilingualStudentProgressService bilingualService)
        {
            _commonService = commonService;
            _ematInfantilService = ematInfantilService;
            _bilingualService = bilingualService;
        }
        
        public IStudentProgressSubjectService Create(string subject)
        {
            StudentProgressSubjectService service;
            switch (subject)
            {
                case SubjectKey.EmatInfantil:
                    service = _ematInfantilService;
                    break;
                case SubjectKey.LudiCat: 
                case SubjectKey.SuperletrasCat:
                    _bilingualService.Subject = subject;
                    service = _bilingualService;
                    break;
                case SubjectKey.Emat:
                case SubjectKey.EmatCat:
                case SubjectKey.EmatMx:
                case SubjectKey.Ludi:
                case SubjectKey.Superletras:
                    _commonService.Subject = subject;
                    service = _commonService;
                    break;
                default:
                    throw new InvalidSubjectException($"The subject ({subject}) doesn't exist.");
            }

            return service;
        }
    }
}