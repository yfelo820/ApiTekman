using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Services.Students.StudentAnswerService;

namespace Api.Services.Students
{
    public class EmatInfantilStudentAnswerService : IEmatInfantilStudentAnswerService
    {
        private readonly IClaimsService _claims;
        private readonly IContentRepository<Activity> _activities;
        private readonly IParentFeedbackService _feedbackService;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly ISchoolsRepository<StudentProgress> _studentProgress;

        public EmatInfantilStudentAnswerService(
            IContentRepository<Activity> activities,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            ISchoolsRepository<StudentProgress> studentProgress,
            IParentFeedbackService feedbackService,
            IClaimsService claims
        )
        {
            _activities = activities;
            _claims = claims;
            _feedbackService = feedbackService;
            _studentAnswers = studentAnswers;
            _studentProgress = studentProgress;
        }

        public async Task<StudentAnswerDTO> AddStudentAnswer(StudentAnswerDTO studentAnswer)
        {
            var userName = _claims.GetUserName();
            var languageKey = _claims.GetLanguageKey();
            var activity = await GetActivityWithSubjectAndCourse(studentAnswer.ActivityId);

            studentAnswer.StudentGrade = studentAnswer.Grade;
            studentAnswer.Session %= activity.Subject.SessionCount;
            if (studentAnswer.Session == 0) studentAnswer.Session = activity.Subject.SessionCount;
            
            // -- SAVING STUDENT ANSWER --
            // If the user has already passed this session/stage, we don't save the new intent.
            var oldPassedStudentAnswer = await GetOldPassedStudentAnswer(activity, studentAnswer, userName, languageKey);
            if (oldPassedStudentAnswer != null)
            {
                // the student can improve his/her grade
                oldPassedStudentAnswer.StudentGrade = oldPassedStudentAnswer.StudentGrade >= studentAnswer.StudentGrade ?
                                                      oldPassedStudentAnswer.StudentGrade : studentAnswer.StudentGrade;
                await UpdateStudentAnswer(oldPassedStudentAnswer);
                // The teacher must see the first attempt
                studentAnswer.Grade = oldPassedStudentAnswer.Grade;
                await UpdateStudentProgress(studentAnswer, userName, languageKey, activity);
                return studentAnswer;
            }

            // If the student has not passed the session/stage yet, we create a new studentAnswer with the grade.
            await AddNewStudentAnswer(activity, studentAnswer, userName, languageKey);

            await UpdateStudentProgress(studentAnswer, userName, languageKey, activity);
            return studentAnswer;
        }

        private async Task UpdateStudentProgress(StudentAnswerDTO studentAnswer, string userName, string languageKey, Activity activity)
        {
            // -- UPDATING STUDENT PROGRESS --
            // If the students have passed the stage, we will check if they can proceed to the next session.
 
            // Check if the whole session is completed and update student progress if it is.
            var currentProgress = await GetStudentProgress(userName, languageKey);
            if (currentProgress.IsProgressing(studentAnswer.Session,activity.Course.Number))
            {
                // If the progress is the current session or a previous one, we check if all the
                // activities are passed for that session. If they are, we update the student progress
                // to the next session.
                var passedStudentAnswers = await GetPassedStudentAnswersOfSession(
                    activity, studentAnswer, userName, languageKey
                );
                var totalStages = await GetStageCountOfSession(
                    activity.Course.Number, studentAnswer.Session, languageKey
                );
                // If all the activities of the session have been passed, we update the student
                // progress to the next session.
                if (passedStudentAnswers >= totalStages)
                {
                    // Notify a possible parent feedback. 
                    await _feedbackService.NotifyFeedbackIfNecessary(currentProgress);
                    var courseSessions = activity.Subject.SessionCount;
                    var subjectCourses = Config.SubjectCourses[SubjectKey.EmatInfantil].End;
                    currentProgress.SetNextSession(courseSessions,subjectCourses);
                    
                    await _studentProgress.Update(currentProgress);
                }
            }
        }

        private async Task<StudentProgress> GetStudentProgress(string userName, string languageKey)
        {
            return await _studentProgress.FindSingle(s =>
                s.UserName == userName
                && s.LanguageKey == languageKey
                && s.SubjectKey == SubjectKey.EmatInfantil
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
                && s.SubjectKey == SubjectKey.EmatInfantil
                && s.LanguageKey == languageKey
                && s.Course == activity.Course.Number
                && s.Session == studentAnswer.Session
                && s.Stage == studentAnswer.Stage
                && s.ActivitySession == activity.Session
                && s.ActivityDifficulty == activity.Difficulty
            );
        }

        // Creates a new student answer for <<Activity>>, on the step in <<studentAnswer>> for <<username>>
        private async Task
        AddNewStudentAnswer(Activity activity, StudentAnswerDTO studentAnswer, string userName, string languageKey)
        {
            await _studentAnswers.Add(new StudentAnswer()
            {
                SubjectKey = SubjectKey.EmatInfantil,
                LanguageKey = languageKey,
                Course = activity.Course.Number,
                Session = studentAnswer.Session,
                Stage = studentAnswer.Stage,

                ActivityId = activity.Id,
                ActivitySession = activity.Session,
                ActivityContentBlockId = activity.ContentBlockId.Value,
                ActivityDifficulty = activity.Difficulty,

                Grade = Math.Max(0, studentAnswer.Grade),
                StudentGrade = Math.Max(0, studentAnswer.StudentGrade),
                CreatedAt = DateTime.Now,
                UserName = userName,

                Level = LevelType.Advance
            });
        }

        /// <summary>
        /// Updates an StudentAnswer
        /// </summary>
        /// <param name="oldPassedStudentAnswer"></param>
        /// <returns></returns>
        private async Task UpdateStudentAnswer(StudentAnswer oldPassedStudentAnswer)
        {
            await _studentAnswers.Update(oldPassedStudentAnswer);
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
                && s.SubjectKey == SubjectKey.EmatInfantil
            )).Count;
        }

        // Gets the number of stages a session has
        private async Task<int>
        GetStageCountOfSession(int course, int session, string languageKey)
        {
            return (await _activities.Find(a =>
                a.Course.Number == course
                && a.Subject.Key == SubjectKey.EmatInfantil
                && a.Language.Code == languageKey
                && a.Session == session
            )).Count;
        }
    }
}