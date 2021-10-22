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
using Api.Interfaces.Students;
using Api.Services.Students.StagesService;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public class EmatStageService : IEmatStageService
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IClaimsService _claimsService;
        private readonly IStageValidator _stageValidator;

        public EmatStageService(
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
            if (stages.ElementAt(stage - 1).Grade >= Config.PassGrade)
            {
                return stages.Where(s => s.Grade < Config.PassGrade).FirstOrDefault();
            }

            return stages.ElementAt(stage - 1);
        }

        public async Task<IEnumerable<StageActivityDTO>> GetAll(int course, int session)
        {
            var userName = _claimsService.GetUserName();
            var language = _claimsService.GetLanguageKey();

            await _stageValidator.Validate(userName, SubjectKey.Emat, language, course, session);

            // [DB QUERIES]
            // Get all student answers potentially involved in the algorithm.
            var studentAnswers = await GetStudentAnswerCandidates(userName, course, session, language);
            // Activity candidates by content block id
            var allActivities = await GetActivityCandidates(course, session, language);
            var activitiesOfSession = allActivities
                .Where(a => a.Session == session && a.Difficulty == Config.MaxDifficultyEmat)
                .OrderBy(a => a.Stage)
                .ToList();

            var result = new List<StageActivityDTO>();
            foreach (var activityOfSession in activitiesOfSession)
            {
                // The content block for this stage.
                var contentBlock = activityOfSession.ContentBlock;
                // All the answers the user has sent for this stage.
                var answersOfStage = studentAnswers
                    .Where(s =>
                        s.Stage == activityOfSession.Stage
                        && s.Session == session
                    );
                // The student's answer with the highest grade for this stage.
                var bestAnswer = answersOfStage
                    .OrderByDescending(s => s.Grade)
                    .FirstOrDefault();
                // Number of times a student has failed this stage.
                var numberOfRetries = answersOfStage
                    .Where(a => a.Grade < Config.PassGrade)
                    .Count();
                var activitiesOfContentBlock = allActivities
                    .Where(a => a.ContentBlockId == contentBlock.Id)
                    .ToList();
                var activityAlternatives = activitiesOfContentBlock
                    .Where(a => a.Session == activityOfSession.Session && a.Stage == activityOfSession.Stage)
                    .ToList();
                activityAlternatives.AddRange(
                    activitiesOfContentBlock
                        .Where(a => a.Session != activityOfSession.Session)
                        .ToList()
                );
                var answersOfSession = studentAnswers.Where(s => s.Stage == activityOfSession.Stage).ToList();
                // The activity the student has resolved or has to resolve given the adaptative algorithm.
                var adaptativeActivity = GetActivityByAdaptativeDifficulty(
                    bestAnswer, numberOfRetries, activityAlternatives, answersOfSession
                );
                // Constructing the stage object with all the collected data.
                result.Add(new StageActivityDTO()
                {
                    ActivityId = adaptativeActivity == null ? activityOfSession.Id : adaptativeActivity.Id,
                    Grade = bestAnswer == null ? -1 : bestAnswer.Grade,
                    ContentBlockName = contentBlock.Name,
                    Image = contentBlock.Image,
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
                && s.SubjectKey == SubjectKey.Emat
                && s.LanguageKey == language
                && s.Course == course
                && (
                    // Gets all the student answers of the session
                    s.Session == session
                    // Gets all the student answers of all the activity candidates of the session.
                    || (
                        s.ActivitySession <= session
                        && s.ActivitySession >= session - Config.MaxPreviousSession
                    )
                )
            )
            .ToListAsync();
        }

        // Get all the activity candidates for the adaptative algorithm (all difficulties for
        // session and session-1, and only difficulty 2 for session-2).
        private async Task<List<Activity>>  GetActivityCandidates(int course, 
            int session, string language)
        {
            return await _activities
            .Query(new[] { "ContentBlock" })
            .Where(a =>
                a.Course.Number == course
                && a.Subject.Key == SubjectKey.Emat
                && a.Language.Code == language
                && (
                    a.Session <= session
                    && a.Session >= session - Config.MaxPreviousSession
                )
                && (
                    a.Difficulty == Config.MaxDifficultyEmat
                    || (
                        a.Difficulty == Config.MinDifficulty
                        && a.Session != session - Config.MaxPreviousSession
                    )
                )
                && a.ContentBlockId != null
            )
            .OrderByDescending(a => a.Session)
            .ThenByDescending(a => a.Difficulty)
            .ToListAsync();
        }

        private Activity GetActivityByAdaptativeDifficulty(
            StudentAnswer bestAnswer,
            int numberOfRetries,
            List<Activity> activities,
            List<StudentAnswer> answers
        )
        {
            // If the student has resolved the activity, the activity chosen will be the one
            // in that studentAnswer.
            if (bestAnswer != null && bestAnswer.Grade >= Config.PassGrade)
            {
                return activities.Where(a =>
                    a.Session == bestAnswer.ActivitySession
                    && a.Difficulty == bestAnswer.ActivityDifficulty
                ).FirstOrDefault();
            }

            // If the student has not resolved the activity yet, the one they have to do
            // will be the next activity in the retry list (if it has not been completed yet).
            return GetNextActivityByRetries(numberOfRetries, activities, answers);
        }

        private Activity
        GetNextActivityByRetries(int numberOfRetries, List<Activity> activities, List<StudentAnswer> answers)
        {
            // If there are more retries than activities, the next activity will be a 
            // randowm activity from all of the unresolved (or from all of the availables if there is
            // no unresolved activity).
            if (numberOfRetries >= activities.Count())
            {
                var unresolvedActivities = activities.Where(activity =>
                    !answers.Any(answer => IsCorrectAnswerOfActivity(answer, activity))
                ).ToList();
                if (unresolvedActivities.Count() == 0) unresolvedActivities = activities;
                var randomIdx = (new Random()).Next(0, unresolvedActivities.Count());

                return unresolvedActivities.ElementAt(randomIdx);
            }

            var activityCandidate = activities.ElementAt(numberOfRetries);
            // If the activity has been correctly resolved by another session,
            // we will skip it and get the next candidate.
            if (answers.Any(answer => IsCorrectAnswerOfActivity(answer, activityCandidate)))
            {
                return GetNextActivityByRetries(++numberOfRetries, activities, answers);
            }

            // If not, the correct candidate is found.
            return activityCandidate;
        }

        // Returns true if answer is a passed answer of activity (not checking course nor subject).
        private bool IsCorrectAnswerOfActivity(StudentAnswer answer, Activity activity)
        {
            return answer.ActivityDifficulty == activity.Difficulty
                //&& answer.ContentBlockId == activity.ContentBlockId
                && answer.ActivitySession == activity.Session
                && answer.Grade >= Config.PassGrade;
        }
    }
}
