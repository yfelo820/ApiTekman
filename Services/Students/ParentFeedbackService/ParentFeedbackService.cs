using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.DTO.Teachers;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;

namespace Api.Services.Students.ParentFeedbackService
{
    public class ParentFeedbackService : IParentFeedbackService
    {
        private readonly IClaimsService _claims;
        private readonly IContentRepository<Subject> _subjects;
        private readonly ISchoolsRepository<ParentFeedbackAnswerSet> _parentFeedbackAnswerSet;
        private readonly ISchoolsRepository<ParentFeedbackQuestionSet> _parentFeedbackQuestionSet;
        private readonly ISchoolsRepository<PendingParentFeedback> _pendingParentFeedback;
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly IMapper _mapper;
        private readonly string QuestionSet_TypeA = "A";
        private readonly string QuestionSet_TypeB = "B";

        public ParentFeedbackService(
            IContentRepository<Subject> subjects,
            ISchoolsRepository<ParentFeedbackAnswerSet> parentFeedbackAnswerSet,
            ISchoolsRepository<ParentFeedbackQuestionSet> parentFeedbackQuestionSet,
            ISchoolsRepository<PendingParentFeedback> pendingparentFeedback,
            ISchoolsRepository<StudentGroup> studentGroups,
            IClaimsService claims,
            IMapper mapper)
        {
            _parentFeedbackQuestionSet = parentFeedbackQuestionSet;
            _parentFeedbackAnswerSet = parentFeedbackAnswerSet;
            _pendingParentFeedback = pendingparentFeedback;
            _claims = claims;
            _subjects = subjects;
            _studentGroups = studentGroups;
            _mapper = mapper;
        }

        public async Task<List<ParentFeedbackAnswerDTO>> GetAnswers(string userName)
        {
            var answers = await _parentFeedbackAnswerSet.Find(answer => answer.UserName == userName);

            if (answers.Any())
            {
                return _mapper.Map<List<ParentFeedbackAnswerSet>, List<ParentFeedbackAnswerDTO>>(answers.OrderBy(a => a.FulfillmentDate).ToList());
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the last parent evaluation for a list of students
        /// </summary>
        /// <param name="studentsUsernames"></param>
        /// <returns></returns>
        public async Task<List<ParentFeedbackAverageValuationDTO>> GetAverageValuationsAndComments(List<string> studentsUsernames)
        {
            var result = new List<ParentFeedbackAverageValuationDTO>();

            var answers = await _parentFeedbackAnswerSet.Find(answer => studentsUsernames.Contains(answer.UserName));

            foreach (string userName in studentsUsernames)
            {
                var answersForStudent = answers.Where(a => a.UserName == userName).OrderByDescending(a => a.FulfillmentDate);
                if (answersForStudent.Count() > 0)
                {
                    var lastAnswer = answersForStudent.FirstOrDefault();
                    int averageScore = (lastAnswer.AnswerValue1 + lastAnswer.AnswerValue2 + lastAnswer.AnswerValue3 +
                        lastAnswer.AnswerValue4 + lastAnswer.AnswerValue5) / 5;
                    int commentCount = answersForStudent.Where(a => a.Comments != null && a.Comments.Length > 0).Count();
                    
                    result.Add(new ParentFeedbackAverageValuationDTO
                    {
                        StudentUserName = userName,
                        AverageValue = averageScore,
                        CommentCount = commentCount
                    });
                }
            }

            return result;
        }

        public async Task<ParentFeedbackQuestionDTO> GetQuestionSet(string userName)
        {
            var answerCount = _parentFeedbackAnswerSet.Query().Where(answSet => answSet.UserName == userName).Count();
            var questionType = QuestionSet_TypeB;
            if (answerCount == 0 || answerCount % 2 == 0) questionType = QuestionSet_TypeA;

            var questionSet = await _parentFeedbackQuestionSet.FindSingle(qs => qs.QuestionSetType == questionType);

            return _mapper.Map<ParentFeedbackQuestionSet, ParentFeedbackQuestionDTO>(questionSet);            
        }

        public Task Put(ParentFeedbackAnswerDTO parentFeedbackAnswer)
        {
            var parentFeedback = _mapper.Map<ParentFeedbackAnswerDTO, ParentFeedbackAnswerSet>(parentFeedbackAnswer);
            parentFeedback.Id = Guid.NewGuid();
            parentFeedback.FulfillmentDate = DateTime.UtcNow;
            return _parentFeedbackAnswerSet.Add(parentFeedback);
        }

        /// <summary>
        /// Determines if a new parent feedback has to be notified.
        /// IMPORTANT: currently this code only works for Emat and Infantil. 
        /// </summary>
        /// <param name="currentProgress"></param>
        /// <returns></returns>
        public async Task NotifyFeedbackIfNecessary(StudentProgress currentProgress)
        {
            var userName = _claims.GetUserName();
            var languageKey = _claims.GetLanguageKey();
            var subjectKey = _claims.GetSubjectKey();
            var group = await GetGroup(userName, subjectKey, languageKey);
            var subject = await GetSubject(subjectKey);

            // Is parent feedback enabled for the group
            if (group.ParentRating)
            {
                // An array containing all courses for the subjectKey of the student
                var courses = Enumerable.Range(Config.SubjectCourses[subjectKey].Start, Config.SubjectCourses[subjectKey].End - Config.SubjectCourses[subjectKey].Start + 1)
                                        .ToArray();
                // Calculates how many sessions have been done by the student, taking into account in what course is he/she now.
                // Ex.: If student is in course 14, it is the second course of "Infantil" (courses would be [13, 14, 15]).
                // So the student has done all sessions of the first course plus his/her current session.

                var sessions = courses.IndexOf(currentProgress.Course) * subject.SessionCount + currentProgress.Session;

                //var sessions = isCoursePassed ? currentProgress.Session + subject.SessionCount
                //                              : currentProgress.Session;

                // Check if we have to notify a new parent feedback
                if (sessions % Config.FeedbackSessionCount == 0)
                {
                    await UpsertPendingFeedback(new PendingParentFeedbackDTO { UserName = userName });
                }
            }
        }

        public async Task UpsertPendingFeedback(PendingParentFeedbackDTO pedingFeedback)
        {
            var currentFeedback = await _pendingParentFeedback.FindSingle(ppf => ppf.UserName == pedingFeedback.UserName);
            if(currentFeedback != null)
            {
                currentFeedback.RequestTime = DateTime.UtcNow;
                currentFeedback.IsRead = pedingFeedback.IsRead;
                await _pendingParentFeedback.Update(currentFeedback);
            }
            else
            {
                currentFeedback = new PendingParentFeedback
                {
                    RequestTime = DateTime.UtcNow,
                    UserName = pedingFeedback.UserName,
                    IsRead = false
                };

                await _pendingParentFeedback.Add(currentFeedback);
            }
        }

        public async Task DeletePendingFeedback(string userName)
        {
            var currentFeedback = await _pendingParentFeedback.FindSingle(ppf => ppf.UserName == userName);
            if (currentFeedback != null)
            {
                await _pendingParentFeedback.Delete(currentFeedback);
            }
        }

        public async Task<List<PendingParentFeedbackDTO>> GetPendingFeedback(List<string> studentsId)
        {
            var pendingFeedbacks = await _pendingParentFeedback.Find(ppf => studentsId.Contains(ppf.UserName));
            var result = new List<PendingParentFeedbackDTO>();
            if (pendingFeedbacks.Any())
            {
                pendingFeedbacks.ForEach(pendingFeedback =>
                {
                    result.Add(new PendingParentFeedbackDTO
                    {
                        UserName = pendingFeedback.UserName,
                        RequestTime = pendingFeedback.RequestTime,
                        IsRead = pendingFeedback.IsRead
                    });
                });
            }

            return result;
        }


        /// <summary>
        /// Gets a group based on the student name and language.
        /// </summary>
        private async Task<Group> GetGroup(string userName, string subjectKey, string languageKey)
        {
            return (await _studentGroups.FindSingle(s =>
               s.UserName == userName
               && s.Group.SubjectKey == subjectKey
               && s.Group.LanguageKey == languageKey,
                new[] { "Group" })
            ).Group;
        }

        /// <summary>
        /// Gets a subject based on the student's subjectKey.
        /// </summary>
        private async Task<Subject> GetSubject(string subjectKey)
        {
            return await _subjects.FindSingle(s => s.Key.ToLower() == subjectKey.ToLower());
        }
    }
}
