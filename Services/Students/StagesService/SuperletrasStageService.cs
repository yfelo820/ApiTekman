using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students.StagesService
{
    public class SuperletrasStageService : ISuperletrasStageService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IClaimsService _claims;
        private readonly IStageValidator _stageValidator;

        public SuperletrasStageService(
            IContentRepository<Activity> activities,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            IClaimsService claims,
            IStageValidator stageValidator
        )
        {
            _activities = activities;
            _studentAnswers = studentAnswers;
            _claims = claims;
            _stageValidator = stageValidator;
        }

        public async Task<StageActivityDTO> GetNext(int course, int session, int stage)
        {
            return (await GetAll(course, session + 1)).First();
        }

        public async Task<IEnumerable<StageActivityDTO>> GetAll(int course, int session)
        {
            var userName = _claims.GetUserName();
            var subjectKey = _claims.GetSubjectKey();
            var languageKey = _claims.GetLanguageKey();

            await _stageValidator.Validate(userName, subjectKey, languageKey, course, session);

            var activity = await GetStageActivity(subjectKey, languageKey, userName, course, session);
            var grade = await GetGradeOfStage(subjectKey, languageKey, userName, course, session, activity.ContentBlockId.Value);
            return new List<StageActivityDTO>(){
                new StageActivityDTO()
                {
                    ActivityId = activity.Id,
                    Grade = grade,
                    ContentBlockName = activity.ContentBlock.Name,
                    Image = activity.ContentBlock.Image,
                    Stage = activity.Stage
                }
            };
        }
        
        private async Task<Activity> GetStageActivity(string subjectKey, string languageKey, string userName, 
            int course, int session)
        {
            var activities = await GetActivitiesFromSession(subjectKey, languageKey, course, session);
            if (!activities.Any())
            {
                throw new HttpException(HttpStatusCode.BadRequest);
            }
            var currentDifficulty = await GetSessionDifficultyLevel(
                subjectKey, languageKey, userName, course, session, activities.First().ContentBlockId.Value
            );

            var activity = activities.FirstOrDefault(a => a.Difficulty == currentDifficulty);
            if (activity == null)
            {
                activity = activities.FirstOrDefault(a => a.Difficulty == Config.DefaultDifficultyLudi) ?? 
                    throw new HttpException(HttpStatusCode.BadRequest);
            }

            return activity;
        }

        private async Task<List<Activity>> GetActivitiesFromSession(string subjectKey, string languageKey, int course, 
            int session)
        {
            return await _activities
            .Query(new[] { "ContentBlock", "Course" })
            .Where(a =>
                a.Course.Number == course
                && a.Subject.Key == subjectKey
                && a.Language.Code == languageKey
                && a.Session == session
                && a.ContentBlockId != null
            )
            .ToListAsync();
        }

        private async Task<int> GetSessionDifficultyLevel(string subjectKey, string languageKey, string userName, 
            int course, int session, Guid contentBlockId)
        {
            var studentAnswer = await _studentAnswers.Query()
                .Where(s =>
                    s.UserName == userName
                    && s.SubjectKey == subjectKey
                    && s.LanguageKey == languageKey
                    && s.Course == course
                    && s.Session < session
                    && s.ActivityContentBlockId == contentBlockId
                )
                .OrderByDescending(s => s.Session)
                .FirstOrDefaultAsync();

            if (studentAnswer == null)
            {
                return Config.DefaultDifficultyLudi;
            }
            if (studentAnswer.Grade >= Config.PassGrade)
            {
                var difficulty = studentAnswer.ActivityDifficulty + 1;
                return Math.Min(difficulty, Config.MaxDifficultyLudi);
            }
            if (studentAnswer.Grade < Config.DescendGrade)
            {
                var difficulty = studentAnswer.ActivityDifficulty - 1;
                return Math.Max(difficulty, Config.MinDifficulty);
            }

            return studentAnswer.ActivityDifficulty;
        }

        private async Task<float> GetGradeOfStage(string subjectKey, string languageKey, string userName, 
            int course, int session, Guid contentBlockId)
        {
            var studentAnswer = await _studentAnswers.FindSingle(s =>
                s.UserName == userName
                && s.SubjectKey == subjectKey
                && s.LanguageKey == languageKey
                && s.Course == course
                && s.Session == session
                && s.ActivityContentBlockId == contentBlockId
            );

            return studentAnswer?.Grade ?? 0;
        }
    }
}
