using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Services.Students.StudentAnswerService;

namespace Api.Services.Students
{
    public class SuperletrasStudentAnswerService : ISuperletrasStudentAnswerService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly ISchoolsRepository<StudentProgress> _studentProgress;
        private readonly IContentRepository<Subject> _subjects;
        private readonly IClaimsService _claims;

        public SuperletrasStudentAnswerService(
            IContentRepository<Activity> activities,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            ISchoolsRepository<StudentProgress> studentProgress,
            IContentRepository<Subject> subjects,
            IClaimsService claims
        )
        {
            _activities = activities;
            _studentAnswers = studentAnswers;
            _studentProgress = studentProgress;
            _subjects = subjects;
            _claims = claims;
        }

        public async Task<StudentAnswerDTO> AddStudentAnswer(StudentAnswerDTO studentAnswer)
        {
            var userName = _claims.GetUserName();
            var activity = await _activities.FindSingle(
                a => a.Id == studentAnswer.ActivityId,
                new[] { "Subject", "Course" }
            );

            studentAnswer.Session = studentAnswer.Session % activity.Subject.SessionCount;
            if (studentAnswer.Session == 0) studentAnswer.Session = activity.Subject.SessionCount;

            

            // -- SAVING STUDENT ANSWER --
            // If the user has already resolved this session/stage, we don't save the new intent
            var oldStudentAnswer = await GetOldPassedStudentAnswer(activity, studentAnswer, userName);
            if (oldStudentAnswer == null)
            {
                await AddNewStudentAnswer(activity, studentAnswer, userName);
            }
            else
            {
                var oldMaxStudentAnswerGrade = Math.Max(oldStudentAnswer.Grade, oldStudentAnswer.StudentGrade ?? 0);
                
                if (studentAnswer.Grade >  oldMaxStudentAnswerGrade)
                {
                    oldStudentAnswer.StudentGrade = studentAnswer.Grade;
                    await _studentAnswers.Update(oldStudentAnswer);
                }
            }
            
            await UpdateStudentProgress(userName,activity);
            
            return studentAnswer;
        }

        private async Task UpdateStudentProgress(string userName, Activity activity)
        {
            var currentProgress = await GetStudentProgress(userName);

            if (currentProgress.IsProgressing(activity.Session, activity.Course.Number))
            {
                var courseSessions = activity.Subject.SessionCount;
                var subjectCourses = Config.SubjectCourses[SubjectKey.Superletras].End;
                currentProgress.SetNextSession(courseSessions,subjectCourses);
                await _studentProgress.Update(currentProgress);
            }
        }

        private Task<StudentProgress> GetStudentProgress(string userName)
        {
            return _studentProgress.FindSingle(s =>
                s.UserName == userName
                && s.LanguageKey == Culture.Es
                && s.SubjectKey == SubjectKey.Superletras
            );
        }
        
        private async Task<StudentAnswer> GetOldPassedStudentAnswer(Activity activity, StudentAnswerDTO studentAnswer, string userName)
        {
            return await _studentAnswers.FindSingle(s =>
                s.UserName == userName
                && s.LanguageKey == Culture.Es
                && s.SubjectKey == SubjectKey.Superletras
                && s.Course == activity.Course.Number
                && s.Session == studentAnswer.Session
                && s.Stage == 0
            );
        }

        // Creates a new student answer for <<Activity>>, on the step in <<studentAnswer>> for <<username>>
        private async Task AddNewStudentAnswer(Activity activity, 
            StudentAnswerDTO studentAnswer, string userName)
        {
            var level = LevelType.Revise;
            if (activity.Difficulty == 2) level = LevelType.Practise;
            else if (activity.Difficulty == 3) level = LevelType.Advance;

            await _studentAnswers.Add(new StudentAnswer()
            {
                SubjectKey = SubjectKey.Superletras,
                LanguageKey = Culture.Es,
                Course = activity.Course.Number,
                Session = studentAnswer.Session,
                Stage = 0,

                ActivityId = activity.Id,
                ActivitySession = activity.Session,
                ActivityContentBlockId = activity.ContentBlockId.Value,
                ActivityDifficulty = activity.Difficulty,

                Grade = Math.Max(0, studentAnswer.Grade),
                CreatedAt = DateTime.Now,
                UserName = userName,

                Level = level
            });
        }
    }
}