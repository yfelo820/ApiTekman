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
using Api.Services.Students.StagesService;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public class LudiStageService : ILudiStageService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IClaimsService _claimsService;
        private readonly IStageValidator _stageValidator;

        public LudiStageService(
            IContentRepository<Activity> activities,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            IClaimsService claimsService,
            IStageValidator stageValidator
        )
        {
            _activities = activities;
            _studentAnswers = studentAnswers;
            _claimsService = claimsService;
            _stageValidator = stageValidator;
        }

        public async Task<StageActivityDTO> GetNext(int course, int session, int stage)
        {
            return (await GetAll(course, session + 1)).First();
        }

        public async Task<IEnumerable<StageActivityDTO>> GetAll(int course, int session)
        {
            var userName = _claimsService.GetUserName();
            var subject = _claimsService.GetSubjectKey();
            var language = _claimsService.GetLanguageKey();

            await _stageValidator.Validate(userName, subject, language, course, session);

            var activity = await GetStageActivity(userName, course, session, subject, language);
            var grade = await GetGradeOfStage(userName, course, session, activity.ContentBlockId.Value, subject);
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

        private async Task<Activity> GetStageActivity(string userName,
            int course, int session, string subject, string language)
        {
            var activities = await _activities
                .Query(new[] { "ContentBlock", "Course" })
                .Where(a =>
                    a.Course.Number == course
                    && a.Subject.Key == subject
                    && a.Language.Code == language
                    && a.Session == session
                    && a.ContentBlockId != null
                ).ToListAsync();

            if (!activities.Any())
            {
                throw new HttpException(HttpStatusCode.BadRequest);
            }
            var currentDifficulty = await GetSessionDifficultyLevel(
                userName, course, session, activities.First().ContentBlockId.Value, subject, language
            );

            var activity = activities.FirstOrDefault(a => a.Difficulty == currentDifficulty);
            if (activity == null)
            {
                activity = activities.FirstOrDefault(a => a.Difficulty == Config.DefaultDifficultyLudi);
                if (activity == null) throw new HttpException(HttpStatusCode.BadRequest);
            }

            return activity;
        }

        private async Task<int> GetSessionDifficultyLevel(string userName,
            int course, int session, Guid contentBlockId, string subject, string language)
        {
            var studentAnswer = await _studentAnswers.Query()
                .Where(s =>
                    s.UserName == userName
                    && s.SubjectKey == subject
                    && s.LanguageKey == language
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

        private async Task<float> GetGradeOfStage(string userName,
            int course, int session, Guid contentBlockId, string subject)
        {
            var studentAnswer = await _studentAnswers.FindSingle(s =>
                s.UserName == userName
                && s.SubjectKey == subject
                && s.Course == course
                && s.Session == session
                && s.ActivityContentBlockId == contentBlockId
            );

            return studentAnswer != null ? studentAnswer.Grade : 0;
        }
    }
}
