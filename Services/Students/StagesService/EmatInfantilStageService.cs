using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Services.Students.StagesService;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public class EmatInfantilStageService : IEmatInfantilStageService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IClaimsService _claimsService;
        private readonly IStageValidator _stageValidator;

        public EmatInfantilStageService(
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
            var stages = await GetAll(course, session);
            if (stages.Count() <= stage)
            {
                // If student has arrived to the final stage (of the session), return null.
                return null;
            }
            return stages.ElementAt(stage);
        }

        public async Task<IEnumerable<StageActivityDTO>> GetAll(int course, int session)
        {
            var userName = _claimsService.GetUserName();
            var language = _claimsService.GetLanguageKey();

            await _stageValidator.Validate(userName, SubjectKey.EmatInfantil, language, course, session);

            // Get all student answers potentially involved in the algorithm.
            var studentAnswers = await GetStudentAnswerCandidates(userName, course, session, language);
            // Activity candidates by content block id
            var allActivities = await GetActivityCandidates(course, session, language);

            var result = new List<StageActivityDTO>();
            foreach (var activityOfSession in allActivities)
            {
                // All the answers the user has sent for this stage.
                var answersOfStage = studentAnswers
                    .Where(s => s.Stage == activityOfSession.Stage
                                && s.Session == session)
                    .OrderBy(s => s.Grade)
                    .FirstOrDefault();

                // Constructing the stage object with all the collected data.
                result.Add(new StageActivityDTO()
                {
                    ActivityId = activityOfSession.Id,
                    // Student must see his/her grade, not the teacher's one
                    Grade = answersOfStage?.StudentGrade ?? -1,
                    ContentBlockName = activityOfSession.ContentBlock.Name,
                    Image = activityOfSession.ContentBlock.Image,
                    Stage = activityOfSession.Stage
                });
            }
            return result;
        }

        // Get all student answers involved in choosing the possible activities of this session
        // or calculating its grade.
        private async Task<List<StudentAnswer>> GetStudentAnswerCandidates(string userName, 
            int course, int session, string language)
        {
            return await _studentAnswers.Query()
            .Where(s =>
                s.UserName == userName
                && s.SubjectKey == SubjectKey.EmatInfantil
                && s.LanguageKey == language
                && s.Course == course
                && s.Session == session
            )
            .ToListAsync();
        }

        // Get all the activities for the session
        private async Task<List<Activity>>  GetActivityCandidates(int course, 
            int session, string language)
        {
            return await _activities
            .Query(new[] { "ContentBlock" })
            .Where(a =>
                a.Course.Number == course
                && a.Subject.Key == SubjectKey.EmatInfantil
                && a.Language.Code == language
                && a.Session == session
                && a.ContentBlockId != null
            )
            .OrderBy(a => a.Session)
            .ThenBy(a => a.Stage)
            .ThenBy(a => a.ContentBlock.Order)
            .ToListAsync();
        }
    }
}
