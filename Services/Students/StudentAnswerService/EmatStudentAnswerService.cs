using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Services.Students.StudentAnswerService;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public class EmatStudentAnswerService : IEmatStudentAnswerService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly ISchoolsRepository<StudentProgress> _studentProgress;
        private readonly IClaimsService _claims;

        public EmatStudentAnswerService(
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
            var activity = await GetActivityWithSubjectAndCourse(studentAnswer.ActivityId);

            studentAnswer.Session = studentAnswer.Session % activity.Subject.SessionCount;
            if (studentAnswer.Session == 0) studentAnswer.Session = activity.Subject.SessionCount;

            // -- SAVING STUDENT ANSWER --
            // If the user has already passed this session/stage, we don't save the new intent.
            var oldPassedStudentAnswer = await GetOldPassedStudentAnswer(activity, studentAnswer, userName, languageKey);
            if (oldPassedStudentAnswer != null)
            {
                studentAnswer.Grade = oldPassedStudentAnswer.Grade;
                await UpdateStudentProgress(studentAnswer, userName, languageKey, activity);
                return studentAnswer;
            }
            var retryCount = await GetRetryCount(activity, studentAnswer, userName, languageKey);
            // If the student has not passed the session/stage yet, we create a new studentAnswer with the grade.
            await AddNewStudentAnswer(activity, studentAnswer, userName, languageKey, retryCount);

            await UpdateStudentProgress(studentAnswer, userName, languageKey, activity);
            return studentAnswer;
        }

        private async Task UpdateStudentProgress(StudentAnswerDTO studentAnswer, string userName, string languageKey, Activity activity)
        {
            // -- UPDATING STUDENT PROGRESS --
            // If the students have passed the stage, we will check if they can proceed to the next session.
            if (studentAnswer.Grade >= Config.PassGrade)
            {
                // Check if the whole session is completed and update student progress if it is.
                var currentProgress = await GetStudentProgress(userName, languageKey);
                if (currentProgress.IsProgressing(studentAnswer.Session,activity.Course.Number))
                {
                    // If the progress is the current session or a previous one, we check if all the
                    // activities are passed for that session. If they are, we update the student progress
                    // to the next session.
                    int passedStudentAnswers = await GetPassedStudentAnswersOfSession(
                        activity, studentAnswer, userName, languageKey
                    );
                    int totalStages = await GetStageCountOfSession(
                        activity.Course.Number, studentAnswer.Session, languageKey
                    );
                    if (passedStudentAnswers >= totalStages)
                    {
                        var courseSessions = activity.Subject.SessionCount;
                        var subjectCourses = Config.SubjectCourses[SubjectKey.Emat].End;
                        currentProgress.SetNextSession(courseSessions,subjectCourses);
                        await _studentProgress.Update(currentProgress);
                    }
                }
            }
        }

        private async Task<StudentProgress> GetStudentProgress(string userName, string languageKey)
        {
            return await _studentProgress.FindSingle(s =>
                s.UserName == userName
                && s.LanguageKey == languageKey
                && s.SubjectKey == SubjectKey.Emat
            );
        }

        // Returns the activity with subject and course
        private async Task<Activity> GetActivityWithSubjectAndCourse(Guid activityId)
        {
            return await _activities.FindSingle(
                a => a.Id == activityId,
                new[] { "Subject", "Course" }
            );
        }

        // Returns the last passed student answer of <<userName>> for the activity <<activity>>
        private async Task<StudentAnswer>
        GetOldPassedStudentAnswer(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey)
        {
            return await _studentAnswers.FindSingle(s =>
                s.UserName == userName
                && s.SubjectKey == SubjectKey.Emat
                && s.LanguageKey == languageKey
                && s.Course == activity.Course.Number
                && s.Session == studentAnswer.Session
                && s.Stage == studentAnswer.Stage
                && s.ActivitySession == activity.Session
                && s.ActivityDifficulty == activity.Difficulty
                && s.Grade >= Config.PassGrade
            );
        }

        private async Task<int>
        GetRetryCount(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey)
        {
            return await _studentAnswers.Query().Where(s =>
                s.UserName == userName
                && s.SubjectKey == SubjectKey.Emat
                && s.LanguageKey == languageKey
                && s.Course == activity.Course.Number
                && s.Session == studentAnswer.Session
                && s.Stage == studentAnswer.Stage
            ).CountAsync();
        }

        // Creates a new student answer for <<Activity>>, on the step in <<studentAnswer>> for <<username>>
        private async Task
        AddNewStudentAnswer(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey, int retryCount)
        {
            var level = LevelType.ReviseAgain;
            if (retryCount == 0) level = LevelType.Advance;
            else if (retryCount == 1) level = LevelType.Practise;
            else if (retryCount == 2) level = LevelType.Revise;

            await _studentAnswers.Add(new StudentAnswer()
            {
                SubjectKey = SubjectKey.Emat,
                LanguageKey = languageKey,
                Course = activity.Course.Number,
                Session = studentAnswer.Session,
                Stage = studentAnswer.Stage,

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

        // Gets the number of passed stages of a session
        private async Task<int>
        GetPassedStudentAnswersOfSession(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey)
        {
            return (await _studentAnswers.Find(s =>
                s.UserName == userName
                && s.LanguageKey == languageKey
                && s.Course == activity.Course.Number
                && s.Session == studentAnswer.Session
                && s.SubjectKey == SubjectKey.Emat
                && s.Grade >= Config.PassGrade
            )).Count;
        }

        // Gets the number of stages a session has
        private async Task<int>
        GetStageCountOfSession(int course, int session, string languageKey)
        {
            return (await _activities.Find(a =>
                a.Course.Number == course
                && a.Subject.Key == SubjectKey.Emat
                && a.Language.Code == languageKey
                && a.Difficulty == Config.MaxDifficultyEmat
                && a.Session == session
            )).Count;
        }
    }
}