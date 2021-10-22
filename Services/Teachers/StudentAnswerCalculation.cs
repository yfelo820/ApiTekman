using System;
using System.Collections.Generic;
using System.Linq;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Schools;

namespace Api.Services.Teachers
{
    public static class StudentAnswerCalculation
    {
        private const int SessionsForAverageGrade = 10;
        private const float MinPassGrade = 0.5f;
        private const int StagesInSessionEmat = 6;

        public static int CalculateCompletedEmatSessions(IEnumerable<StudentAnswer> studentAnswers)
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
                    if (sessionsFromSameStageCount == StagesInSessionEmat) ++completedSessionCount;
                }
                else sessionsFromSameStageCount = 1;
            }
            return completedSessionCount;
        }

        public static int CalculateCompletedLudiSessions(IEnumerable<StudentAnswer> studentAnswers)
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

        public static float CalculateAverageGrade(IEnumerable<StudentAnswer> studentAnswers)
        {
            // Gets the max grade & the mean grades for the 10 last sessions
            var studentAnswersByStage = studentAnswers
                .OrderByDescending(s => s.Course)
                .ThenByDescending(s => s.Session)
                .ThenByDescending(s => s.Stage)
                .GroupBy(s => new
                {
                    s.Course,
                    s.Session,
                    s.Stage
                })
                .Select(s => new
                {
                    MeanGrade = s.Sum(so => so.Grade) / s.Count(),
                    MaxGrade = s.Max(so => so.Grade)
                })
                .Take(SessionsForAverageGrade)
                .ToList();



            if (!studentAnswersByStage.Any())
            {
                return 0;
            }

            var sumGrades = 0.0f;
            foreach (var answer in studentAnswersByStage)
            {
                var grade = answer.MeanGrade;
                if (answer.MaxGrade >= Config.PassGrade) grade = MathF.Max(grade, MinPassGrade);
                sumGrades += grade;
            }

            return (sumGrades / studentAnswersByStage.Count) * 10;
        }
    }
}
