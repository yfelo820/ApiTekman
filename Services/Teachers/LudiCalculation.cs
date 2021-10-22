using System.Collections.Generic;
using System.Linq;
using Api.Entities.Schools;

namespace Api.Services.Teachers
{
    public class LudiCalculation : Calculation
    {
        public override int CalculateCompletedSessions(IEnumerable<StudentAnswer> studentAnswers)
        {
            return studentAnswers
                .Select(s => new
                {
                    s.Course,
                    s.Session,
                    s.LanguageKey
                })
                .Distinct()
                .Count();
        }
    }
}
