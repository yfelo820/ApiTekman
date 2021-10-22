using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Services.Teachers
{
    public interface IStudentAnswerCalculation
    {
        float CalculateAverageGrade(IEnumerable<StudentAnswer> studentAnswers, Guid? contentBlockId = null);
        int CalculateCompletedSessions(IEnumerable<StudentAnswer> studentAnswers);
    }
}
