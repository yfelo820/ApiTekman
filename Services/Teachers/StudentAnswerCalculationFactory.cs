using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;

namespace Api.Services.Teachers
{
    public static class StudentAnswerCalculationFactory
    {
        public static IStudentAnswerCalculation CreateFactory(string subjectKey)
        {
            switch (subjectKey)
            {
                case SubjectKey.Emat:
                    return new EmatCalculation();
                case SubjectKey.EmatInfantil:
                    return new EmatInfantilCalculation();
                case SubjectKey.Ludi:
                    return new LudiCalculation();
                default:
                    return new EmatCalculation();
            }
        }
    }
}
