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
    public class LudiStudentAnswerService : ILudiStudentAnswerService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly ISchoolsRepository<StudentProgress> _studentProgress;
        private readonly IClaimsService _claims;

        public LudiStudentAnswerService(
            IContentRepository<Activity> activities,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            ISchoolsRepository<StudentProgress> studentProgress,
            IClaimsService claims
        )
        {
            _activities = activities;
            _studentAnswers = studentAnswers;
            _studentProgress = studentProgress;
            _claims = claims;
        }

        public async Task<StudentAnswerDTO> AddStudentAnswer(StudentAnswerDTO studentAnswer)
        {
            var userName = _claims.GetUserName();
            var languageKey = _claims.GetLanguageKey();
            var subjectKey = _claims.GetSubjectKey();
            var ludiSessionsBySubject = (subjectKey == SubjectKey.LudiCat) ? Config.LudiCatSessions : Config.LudiSessions;

            studentAnswer.Session = studentAnswer.Session % ludiSessionsBySubject[0];
            if (studentAnswer.Session == 0) studentAnswer.Session = ludiSessionsBySubject[0];

            var activity = await  _activities.FindSingle(
                a => a.Id == studentAnswer.ActivityId,
                new[] { "Course" }
            );

            // -- SAVING STUDENT ANSWER --
            // If the user has already resolved this session/stage, we don't save the new intent
            var oldStudentAnswer = await GetOldPassedStudentAnswer(activity, studentAnswer, userName, languageKey);

            if (oldStudentAnswer == null)
            {
                await AddNewStudentAnswer(activity, studentAnswer, userName, languageKey);
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

            await UpdateStudentProgress(studentAnswer,userName,languageKey, activity);
            
            return studentAnswer;
        }

        private async Task<StudentProgress> GetStudentProgress(string userName, string languageKey, string subjectKey)
        {
            return await _studentProgress.FindSingle(s =>
                s.UserName == userName
                && s.LanguageKey == languageKey
                && s.SubjectKey == subjectKey
            );
        }

        private async Task UpdateStudentProgress(StudentAnswerDTO studentAnswer,string userName, string languageKey, Activity activity)
        {
            var subjectKey = _claims.GetSubjectKey();
            var currentProgress = await GetStudentProgress(userName, languageKey, subjectKey);
            
            if (currentProgress.IsProgressing(activity.Session,activity.Course.Number))
            {
                var courseSessions = GetCourseSessions(subjectKey, activity.Course.Number);
                var subjectCourses = Config.SubjectCourses[subjectKey].End;
                currentProgress.SetNextSession(courseSessions,subjectCourses);
                await _studentProgress.Update(currentProgress);  
            }
        }

        private int GetCourseSessions(string subjectKey, int activityCourse)
        {
            return subjectKey == SubjectKey.Ludi ?
                Config.LudiSessions[activityCourse - 1] :
                Config.LudiCatSessions[activityCourse - 1];
        }
        
        private async Task<StudentAnswer> GetOldPassedStudentAnswer(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey)
        {
            return await _studentAnswers.FindSingle(s =>
                s.UserName == userName
                && s.LanguageKey == languageKey
                && s.SubjectKey == _claims.GetSubjectKey()
                && s.Course == activity.Course.Number
                && s.Session == studentAnswer.Session
                && s.Stage == 0
            );
        }

        // Creates a new student answer for <<Activity>>, on the step in <<studentAnswer>> for <<username>>
        private async Task
        AddNewStudentAnswer(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey)
        {
            var level = LevelType.Revise;
            if (activity.Difficulty == 2) level = LevelType.Practise;
            else if (activity.Difficulty == 3) level = LevelType.Advance;

            await _studentAnswers.Add(new StudentAnswer()
            {
                SubjectKey = _claims.GetSubjectKey(),
                LanguageKey = languageKey,
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