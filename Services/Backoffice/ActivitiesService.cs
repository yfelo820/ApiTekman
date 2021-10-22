using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Backoffice;
using Api.DTO.Backoffice.ProblemResolution;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Backoffice
{
    public class ActivitiesService : IApiService<Activity>
    {
        private readonly IContentRepository<Activity> _activities;
        private readonly IContentRepository<Exercise> _exercises;
        private readonly ExercisesService _exercisesService;
        private readonly IContentRepository<DragDrop> _dragDrop;
        private readonly SceneService<Exercise> _sceneService;
        private readonly IBlobStorageService _blob;
        private readonly ISubjectsService _subjectsService;
        private readonly ILanguagesService _languagesService;
        private readonly ICoursesService _coursesService;
        private readonly IStudentProblemsAnswerService _studentProblemsAnswerService;

        public ActivitiesService(
            IContentRepository<Activity> activityRepo,
            IContentRepository<Exercise> exerciseRepo,
            IApiService<Exercise> exercisesService,
            IContentRepository<DragDrop> dragDrop,
            IBlobStorageService blob,
            ISubjectsService subjectsService,
            ILanguagesService languagesService,
            ICoursesService coursesService,
            IStudentProblemsAnswerService studentProblemsAnswerService
        )
        {
            _activities = activityRepo;
            _exercises = exerciseRepo;
            _exercisesService = exercisesService as ExercisesService;
            _dragDrop = dragDrop;
            _blob = blob;
            _sceneService = new SceneService<Exercise>(null, null, null, blob);
            _subjectsService = subjectsService;
            _languagesService = languagesService;
            _coursesService = coursesService;
            _studentProblemsAnswerService = studentProblemsAnswerService;
        }

        public async Task<List<Activity>> Filter(Guid? subjectId, Guid? languageId, Guid? courseId = null, Guid? problemResolutionId = null)
        {
            var activities = _activities.Query(new[] { "Course", "ContentBlock", "ProblemResolution" });

            if (subjectId.HasValue)
            {
                activities = activities.Where(a => a.SubjectId == subjectId);
            }

            if (languageId.HasValue)
            {
                activities = activities.Where(a => a.LanguageId == languageId);
            }

            if (courseId.HasValue)
            {
                activities = activities.Where(a => a.CourseId == courseId);
            }

            if (problemResolutionId.HasValue)
            {
                activities = activities.Where(a => a.ProblemResolutionId == problemResolutionId.Value);
            }

            return await activities.OrderBy(a => a.Course.Number)
                .ThenBy(a => a.Session)
                .ThenBy(a => a.Stage)
                .ThenBy(a => a.Difficulty)
                .ToListAsync();
        }

        public async Task<Activity> GetSingle(Guid id)
        {
            return await _activities.FindSingle(
                a => a.Id == id,
                new[] { "Subject", "Course", "Language", "ContentBlock", "ProblemResolution" }
            );
        }

        public async Task<Activity> Add(Activity activity)
        {
            Validate(activity);
            RemoveVirtuals(activity);
            return await _activities.Add(activity);
        }

        public async Task
        CopyFromActivity(List<CopyActivityDTO> copyActivities, Guid subjectId, Guid languageId)
        {
            List<Activity> newActivities = new List<Activity>();
            List<Activity> activities = _activities.Query(new[]
            {
                "Exercises.Items",
                "Exercises.Items.Style"
            })
            .AsNoTracking()
            .ToList();

            List<DragDrop> dragDropList = await _dragDrop.GetAll();

            Activity activity;
            foreach (CopyActivityDTO copyActivity in copyActivities)
            {
                activity = activities.FirstOrDefault(a => a.Id == copyActivity.ActivityId);

                activity.ContentBlockId = copyActivity.ContentBlockId;
                activity.LanguageId = languageId;
                activity.SubjectId = subjectId;

                activity.Id = Guid.NewGuid();
                foreach (Exercise exercise in activity.Exercises)
                {
                    exercise.Id = Guid.NewGuid();
                    string path = "activities/" + activity.Id + "/" + activity.Id + "/";
                    exercise.BackgroundImage = await _blob.CopyFileByUrl(exercise.BackgroundImage, path);
                    exercise.Thumbnail = await _blob.CopyFileByUrl(exercise.Thumbnail, path);
                    exercise.ActivityId = Guid.Empty;
                    foreach (Item item in exercise.Items)
                    {
                        item.MediaUrl = await _blob.CopyFileByUrl(item.MediaUrl, path);
                        if (item.Type == ItemType.Drop)
                        {
                            List<DragDrop> dragDropListFilter = dragDropList.Where(i => i.ItemDropId == item.Id).ToList();
                            List<DragDrop> newDragDropList = new List<DragDrop>();
                            foreach (DragDrop dragDrop in dragDropListFilter)
                            {
                                newDragDropList.Add(new DragDrop()
                                {
                                    ItemDragId = dragDrop.ItemDragId,
                                    ItemDropId = dragDrop.ItemDropId,
                                    MultipleDragResult = dragDrop.MultipleDragResult
                                });
                            }
                            ItemDrop itemDrop = item as ItemDrop;
                            itemDrop.DragAnswers = newDragDropList;
                        }
                    }
                    _sceneService.CleanForeignKeys(exercise);
                }
                newActivities.Add(activity);
            }
            await _activities.Add(newActivities);
        }

        public async Task<Activity> Delete(Guid id)
        {
            Activity activity = await GetSingle(id);
            List<Guid> exerciseIds = await _exercises.Query().AsNoTracking()
                .Where(e => e.ActivityId == id)
                .Select(e => e.Id)
                .ToListAsync();
            foreach (Guid exerciseId in exerciseIds) await _exercisesService.Delete(exerciseId);
            await _activities.Delete(activity);
            return activity;
        }

        public async Task<Activity> Update(Activity activity)
        {
            Validate(activity);
            RemoveVirtuals(activity);
            await _activities.Update(activity);
            return activity;
        }

        public Task<List<Activity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProblemResolutionActivitiesDTO>> GetProblemResolutionActivities(Group group, Guid? problemResolutionId)
        {
            var subjectId = (await _subjectsService.GetSingle(group.SubjectKey)).Id;
            var languageId = (await _languagesService.GetSingle(group.LanguageKey)).Id;
            var courseId = group.AccessAllCourses ? (Guid?)null : (await _coursesService.GetSingle(group.Course)).Id;

            var activities = _activities.Query().Where(a => a.SubjectId == subjectId && a.LanguageId == languageId);

            if (problemResolutionId.HasValue)
            {
                activities = activities.Where(a => a.ProblemResolutionId == problemResolutionId.Value);
            }

            if (courseId.HasValue)
            {
                activities = activities.Where(a => a.CourseId == courseId);
            }

            activities = activities.OrderBy(a => a.Course.Number).ThenBy(a => a.Session).ThenBy(a => a.Stage);

            var answers = await _studentProblemsAnswerService.GetAll();

            var result = await activities.Select(a => new ProblemResolutionActivitiesDTO
            {
                Id = a.Id,
                Course = a.Course.Number,
                Session = a.Session,
                Stage = a.Stage,
                State = answers.Find(item => item.ActivityContentBlockId.Equals(a.Id)) != null ? 0 : 1
            }).ToListAsync();

            return result;
        }

        /// <summary>
        ///  Checks if, for a given activity, exists at least an activity of default difficulty for
        ///  that same stage.
        /// </summary>
        public async Task<bool> IsDifficultyValid(Guid id)
        {
            var activity = await _activities.Get(id);
            return await _activities.Any(a =>
                a.LanguageId == activity.LanguageId
                && a.SubjectId == activity.SubjectId
                && a.CourseId == activity.CourseId
                && a.Session == activity.Session
                && a.Stage == activity.Stage
                && a.Difficulty == Config.DefaultDifficultyLudi
            );
        }

        private void Validate(Activity activity)
        {
            if (!activity.IsValid) throw new Exception();
        }

        private void RemoveVirtuals(Activity activity)
        {
            activity.Course = null;
            activity.Subject = null;
            activity.Language = null;
            activity.ContentBlock = null;
            activity.ProblemResolution = null;
        }
    }
}