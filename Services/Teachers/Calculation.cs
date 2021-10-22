using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Schools;

namespace Api.Services.Teachers
{
    public abstract class Calculation : IStudentAnswerCalculation
    {
        private int _sessionsForAverageGrade = 10;
        private float _minPassGrade = 0.5f;
        

        protected Calculation() { }

        protected Calculation(int sessionForAverageGrade, int minPassGrade)
        {
            _sessionsForAverageGrade = sessionForAverageGrade;
            _minPassGrade = minPassGrade;
        }

        public abstract int CalculateCompletedSessions(IEnumerable<StudentAnswer> studentAnswers);

        // Gets the max grade & the mean grades for the n last sessions
        public float CalculateAverageGrade(IEnumerable<StudentAnswer> studentAnswers, Guid? contentBlockId = null)
        {
            var studentAnswersByStage = GetStudentAnswerByStage(studentAnswers, contentBlockId);
            if (!studentAnswersByStage.Any())
            {
                return 0;
            }

            var sumGrades = 0.0f;
            foreach (var answer in studentAnswersByStage)
            {
                var grade = answer.MeanGrade;
                if (answer.MaxGrade >= Config.PassGrade) grade = MathF.Max(grade, _minPassGrade);
                sumGrades += grade;
            }

            return (sumGrades / studentAnswersByStage.Count) * 10; // TODO: check 10
        }

        private List<Grades> GetStudentAnswerByStage(IEnumerable<StudentAnswer> studentAnswers, Guid? contentBlockId = null)
        {
            var studentAnswersByContent = contentBlockId == null ? studentAnswers : studentAnswers.Where(sa => sa.ActivityContentBlockId.Equals(contentBlockId));
            return studentAnswersByContent
                .OrderByDescending(s => s.Course)
                .ThenByDescending(s => s.Session)
                .ThenByDescending(s => s.Stage)
                .GroupBy(s => new
                {
                    s.Course,
                    s.Session,
                    s.Stage
                })
                .Select(s => new Grades
                {
                    MeanGrade = s.Sum(so => so.Grade) / s.Count(),
                    MaxGrade = s.Max(so => so.Grade)
                })
                .Take(_sessionsForAverageGrade)
                .ToList();
        }
    }
}
