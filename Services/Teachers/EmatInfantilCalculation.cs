using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Schools;

namespace Api.Services.Teachers
{
    public class EmatInfantilCalculation : Calculation
    {

        private const int StagesInSessionEmatInfantil = 7;

        // Cuanta el número de activityblocks terminados, si llega a 6 suma 1 a las sesiones terminadas
        public override int CalculateCompletedSessions(IEnumerable<StudentAnswer> studentAnswers)
        {
            var completedStages = studentAnswers
                .Select(s => new
                {
                    s.Course,
                    s.Session,
                    s.Stage
                })
                .Distinct()
                .OrderBy(s => s.Course)
                .ThenBy(s => s.Session)
                .ToArray();
            var completedSessionCount = 0;
            var sessionsFromSameStageCount = 1;
            for (var i = 1; i < completedStages.Length; ++i)
            {
                var stage = completedStages[i];
                var previousStage = completedStages[i - 1];
                if (stage.Session == previousStage.Session && stage.Course == previousStage.Course)
                {
                    ++sessionsFromSameStageCount;
                    if (sessionsFromSameStageCount == StagesInSessionEmatInfantil) ++completedSessionCount;
                }
                else sessionsFromSameStageCount = 1;
            }
            return completedSessionCount;
        }
    }
}
