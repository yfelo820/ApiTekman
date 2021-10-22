using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using Api.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public interface IItineraryService
    {
        Task<ItineraryDTO> GetItinerary();
    }

    public class ItineraryService : IItineraryService
    {
        private readonly ISchoolsRepository<StudentProgress> _studentProgress;
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IContentRepository<Activity> _activities;
        private readonly ISubjectsService _subjectService;
        private readonly IClaimsService _claimsService;


        public ItineraryService(
            ISchoolsRepository<StudentProgress> studentProgress,
            ISchoolsRepository<StudentGroup> studentGroups,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            IContentRepository<Activity> activities,
            IClaimsService claimsService,
            ISubjectsService subjectService)
        {
            _studentGroups = studentGroups;
            _studentProgress = studentProgress;
            _studentAnswers = studentAnswers;
            _activities = activities;
            _claimsService = claimsService;
            _subjectService = subjectService;
        }

        public async Task<ItineraryDTO> GetItinerary()
        {
            var subjectKey = _claimsService.GetSubjectKey();
            var language = _claimsService.GetLanguageKey();
            var userName = _claimsService.GetUserName();

            var subject = await _subjectService.GetSubject(subjectKey);
            var group = await GetGroup(subjectKey, language, userName);
            var studentProgress = await GetStudentProgress(subjectKey, userName, language);
            var studentAnswers = await GetStudentAnswers(subjectKey, userName, language);

            var sessions = new List<ItinerarySessionDTO>();
            var sessionKeyCount = 1;
            foreach (var course in subject.Courses)
            {
                var publishedSessions = await GetSessions(subjectKey, language, course.Number);
                for (var session = 1; session <= course.SessionCount; session++)
                {
                    var state = GetSessionState(subject, course.Number, session, group, studentProgress,
                        studentAnswers,
                        publishedSessions);

                    sessions.Add(new ItinerarySessionDTO
                    {
                        Key = sessionKeyCount, Session = session, Course = course.Number, State = state
                    });

                    sessionKeyCount++;
                }
            }

            var itinerary = new ItineraryDTO
            {
                Sessions = sessions,
                StudentProgressCourse = studentProgress.Course,
                StudentProgressSession = studentProgress.Session
            };

            if (subjectKey == SubjectKey.Emat)
            {
                itinerary.HasDiagnosisTest = await HasDiagnosisTest(group, userName);
            }

            return itinerary;
        }

        // Gets the studentProgress for a student and language.
        private async Task<StudentProgress> GetStudentProgress(string subjectKey, string userName, string languageKey)
        {
            return await _studentProgress.FindSingle(s =>
                s.UserName == userName
                && s.SubjectKey == subjectKey
                && s.LanguageKey == languageKey
            );
        }

        /// <summary>
        /// Gets the student answers of a student and language.
        /// </summary>
        /// <param name="subjectKey"></param>
        /// <param name="userName"></param>
        /// <param name="languageKey"></param>
        private async Task<List<StudentAnswer>> GetStudentAnswers(string subjectKey, string userName, string languageKey)
        {
            return await _studentAnswers.Find(s =>
                s.UserName == userName
                && s.SubjectKey == subjectKey
                && s.LanguageKey == languageKey
            );
        }

        // Gets a group based on the student name and language.
        private async Task<Group> GetGroup(string subjectKey, string languageKey, string userName)
        {
            var studentGroup = await _studentGroups.FindSingle(s =>
                    s.UserName == userName
                    && s.Group.SubjectKey == subjectKey
                    && (subjectKey == SubjectKey.LudiCat || subjectKey == SubjectKey.SuperletrasCat || s.Group.LanguageKey == languageKey),
                new[] {"Group"});

            if (studentGroup == null)
            {
                throw new NotFoundException(
                    $"Group not found for subject '{subjectKey}', language '{languageKey}' and '{userName}'");
            }

            return studentGroup.Group;
        }

        // Gets the SessionState of a sessions course. 
        private SessionState GetSessionState(SubjectResponse subject, int course, int session, Group group,
            StudentProgress progress,
            List<StudentAnswer> studentAnswers, List<int> publishedSessions)
        {
            if (IsSessionLocked(group.SubjectKey, course, session, group, publishedSessions)) return SessionState.Locked;
            if (IsSessionCurrent(course, session, progress)) return SessionState.Current;
            if (IsSessionUnreachable(course, session, progress)) return SessionState.Unreachable;
            if (IsSessionCompleted(subject, course, session, studentAnswers)) return SessionState.Completed;
            return SessionState.Pending;
        }

        private async Task<List<int>> GetSessions(string subjectKey, string languageCode, int course)
        {
             return await _activities.Query()
                .Where(a => a.Course.Number == course && a.Language.Code == languageCode && a.Subject.Key == subjectKey)
                .GroupBy(a => a.Session)
                .Select(g => g.Key)
                .OrderBy(g => g)
                .ToListAsync();
        }

        // Returns true if the session surpasses the session limit of the group (if it has one).
        private bool IsSessionLocked(string subjectKey, int course, int session, Group group, List<int> publishedSessions)
        {
            // SW-4054 [TODO] develop this functionality from the backoffice
            if (group.SubjectKey == SubjectKey.Emat && group.LanguageKey == Culture.Mx &&
                course > Config.SubjectCourses[SubjectKey.EmatMx].End)
            {
                return true;
            }

            if (publishedSessions.All(s => s != session))
            {
                return true;
            }
            
            if (subjectKey != SubjectKey.EmatInfantil)
            {
                if (group.AccessAllSessions) return false;
                return (course == group.LimitCourse && session > group.LimitSession)
                       || course > group.LimitCourse;
            }

            if (group.AccessAllCourses)
            {
                return !group.AccessAllSessions && (course > group.Course
                                                    || (course == group.Course && session > group.LimitSession));
            }

            return course != group.Course
                   || (!group.AccessAllSessions && session > group.LimitSession);
        }

        // Returns true if the session surpasses the current session of the studentProgress.
        private bool IsSessionUnreachable(int course, int session, StudentProgress progress)
        {
            return (course == progress.Course && session > progress.Session)
                   || course > progress.Course;
        }

        // Returns true if the session is the actual session of the studentProgress.
        private bool IsSessionCurrent(int course, int session, StudentProgress progress)
        {
            return course == progress.Course && session == progress.Session;
        }

        // Returns true if the session has all its stages completed (has a studentAnswer per stage).
        private bool IsSessionCompleted(SubjectResponse subject, int course, int session, List<StudentAnswer> studentAnswers)
        {
            var studentAnswersOfSessionCount = studentAnswers
                .Where(s => s.Course == course 
                            && s.Session == session 
                            && (subject.Key == SubjectKey.EmatInfantil 
                                || subject.Key == SubjectKey.Emat && s.Grade >= Config.PassGrade
                                || Math.Max(s.StudentGrade??0,s.Grade) >=  Config.PassGrade ))
                .Select(s => s.Stage)
                .Distinct()
                .Count();

            return studentAnswersOfSessionCount == subject.StagesInSession;
        }

        private async Task<bool> HasDiagnosisTest(Group group, string userName)
        {
            var studentProgress = await GetStudentProgress(group.SubjectKey, userName, group.LanguageKey);
            return studentProgress.DiagnosisTestState == DiagnosisTestState.Pending ||
                   studentProgress.DiagnosisTestState == DiagnosisTestState.NotDefined &&
                   group.FirstSessionWithDiagnosis;
        }
    }
}