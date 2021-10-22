using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Interfaces.Parents;
using Api.Interfaces.Shared;
using Api.Services.Teachers;

namespace Api.Services.Parents
{
    public class TrackingService : ITrackingService
    {

        private readonly IApiService<ContentBlock> _contentBlocksService;
        private readonly IContentRepository<Language> _languages;
        private readonly IContentRepository<Subject> _subjects;
        private readonly ISchoolsRepository<StudentProgress> _progress;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IStudentsService _studentsService;

        public TrackingService(
            IApiService<ContentBlock> contentBlocksService,
            IContentRepository<Language> languages,
            IContentRepository<Subject> subjects,
            ISchoolsRepository<StudentProgress> progress,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            IStudentsService studentsService
        )
        {
            _contentBlocksService = contentBlocksService;
            _languages = languages;
            _subjects = subjects;
            _progress = progress;
            _studentAnswers = studentAnswers;
            _studentsService = studentsService;
        }

        
        public async Task<List<StudentTrackingDTO>> GetMultiples(string subjectKey)
        {
            var students = await _studentsService.GetAllStudents(subjectKey);

            return (await Task.WhenAll(students.Select(async s =>
            {
                var progress = await _progress.FindSingle(p =>
                    p.UserName == s.UserName &&
                    p.SubjectKey == s.SubjectKey &&
                    p.LanguageKey == s.LanguageKey);

                var answers = await _studentAnswers.Find(a =>
                    a.UserName == s.UserName &&
                    a.SubjectKey == s.SubjectKey &&
                    a.LanguageKey == s.LanguageKey);

                return new StudentTrackingDTO()
                {
                    FullName = s.FirstSurname + " " + s.SecondSurname + ", " + s.Name,
                    Photo = s.Photo,
                    UserName = s.UserName,

                    CompletedSessions = s.SubjectKey == SubjectKey.Emat
                                 ? StudentAnswerCalculation.CalculateCompletedEmatSessions(answers)
                                 : StudentAnswerCalculation.CalculateCompletedLudiSessions(answers),
                    AverageScore = StudentAnswerCalculation.CalculateAverageGrade(answers),

                    CurrentSession = progress != null ? progress.Session : 1,
                    CurrentCourse = progress != null ? progress.Course : 0
                };
            }).ToList())).OrderBy(s => s.FullName).ToList();
        }

        public async Task<StudentTrackingExtendedDTO> GetSingleStudentDetail(string userName, string subjectKey)
        {
            //Currently there's no endpoint in TKCore to get a single student, so we get all of the parent's students
            var students = await _studentsService.GetAllStudents(subjectKey);
            var student = students.Single(s => s.UserName == userName);

            var subject = await _subjects.FindSingle(s => s.Key == student.SubjectKey);
            var language = await _languages.FindSingle(l => l.Code == student.LanguageKey);
            var calculator = StudentAnswerCalculationFactory.CreateFactory(student.SubjectKey);
            

            var studentsAnswers = await _studentAnswers.Find(s =>
                s.UserName.Equals(userName)
                && s.LanguageKey == language.Code
                && s.SubjectKey == subject.Key
            );
            var progress = await _progress.FindSingle(p =>
                p.UserName.Equals(userName)
                && p.LanguageKey == language.Code
                && p.SubjectKey == subject.Key
            );

            var allContentBlocks = await _contentBlocksService.GetAll();
            var contentBlocksBySubjectAndLanguage = allContentBlocks.Where(c => c.SubjectId.Equals(subject.Id) && c.LanguageId.Equals(language.Id)).ToList();
            var activityScoreList = new List<ActivityScore>();
            var contentBlocks = studentsAnswers.Select(s => s.ActivityContentBlockId).Distinct().ToList();
            contentBlocksBySubjectAndLanguage.ForEach(c =>
            {
                activityScoreList.Add(new ActivityScore
                {
                    Id = c.Id,
                    Score = calculator.CalculateAverageGrade(studentsAnswers, c.Id),
                    ContentBlockName = c.Name
                });
            });

            return new StudentTrackingExtendedDTO
            {
                Name = student.Name,
                Lastname1 = student.FirstSurname,
                Lastname2 = student.SecondSurname,
                AverageScores = activityScoreList,
                AverageScore = calculator.CalculateAverageGrade(studentsAnswers),
                CompletedSessions = calculator.CalculateCompletedSessions(studentsAnswers),
                CurrentSession = progress?.Session ?? 1
            };
        }

    }
}
