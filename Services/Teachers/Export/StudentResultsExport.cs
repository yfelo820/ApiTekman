using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Api.Resources;

namespace Api.Services.Teachers
{
    public class StudentResultsExport : IStudentResultsExport
    {
        private readonly GroupsService _groupsService;
        private readonly ISchoolsRepository<StudentProgress> _studentProgressRepository;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswerRepository;
        private readonly IUserService _userService;
        private readonly IClaimsService _claimsService;
        private readonly ITrackingService _trackingService;

        private static string[] SummaryHeaders => new string[] {
            Locale.Export_StudentResults_StudentName,
            Locale.Export_StudentResults_AverageScore,
            Locale.Export_StudentResults_CompletedSessions,
            Locale.Export_StudentResults_Course,
            Locale.Export_StudentResults_CurrentSession
        };
        private static string[] ResultsHeaders => new string[] {
            Locale.Export_StudentResults_Course,
            Locale.Export_StudentResults_Session,
            Locale.Export_StudentResults_Stage,
            Locale.Export_StudentResults_Level,
            Locale.Export_StudentResults_Grade
        };

        public StudentResultsExport(
            IApiService<GroupDTO> groupsService,
            ISchoolsRepository<StudentProgress> studentProgressRepository,
            ISchoolsRepository<StudentAnswer> studentAnswerRepository,
            IUserService userService,
            IClaimsService claimsService,
            ITrackingService trackingService)
        {
            _groupsService = groupsService as GroupsService;
            _studentProgressRepository = studentProgressRepository;
            _studentAnswerRepository = studentAnswerRepository;
            _userService = userService;
            _claimsService = claimsService;
            _trackingService = trackingService;
        }

        public async Task<byte[]> Export(Guid groupId)
        {
            var language = _claimsService.GetLanguageKey();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            var group = await _groupsService.GetSingleGroup(groupId);
            var students = await _userService.GetAllStudentsByStage();
            var usernames = group.Students.Select(s => s.UserName);
            var studentsProgress = await _studentProgressRepository.Find(p =>
                usernames.Contains(p.UserName) &&
                p.LanguageKey == group.LanguageKey &&
                p.SubjectKey == group.SubjectKey
            );
            var studentAnswers = (await _studentAnswerRepository.Find(s =>
                usernames.Contains(s.UserName) &&
                s.LanguageKey == group.LanguageKey &&
                s.SubjectKey == group.SubjectKey
            ));

            var studentResult = await GetStudentsResults(group, students, studentsProgress, studentAnswers);
            var body = studentResult.Select(sr => new object[]
            {
                sr.FullName,
                sr.AverageScore,
                sr.CompletedSessions,
                sr.CurrentCourse,
                sr.CurrentSession
            }).ToArray();

            var excelPackageBuilder = new ExcelPackageBuilder()
                .WithTitle(Locale.Export_StudentResults_Title)
                .WithWorkSheet(Locale.Export_StudentResults_SummaryTitle, SummaryHeaders, body);

            var studentAnswersGrouped = studentAnswers
                .OrderBy(sa => sa.Course)
                .ThenBy(sa => sa.Session)
                .ThenBy(sa => sa.Stage)
                .ToList();
            
            group.Students.ForEach(student =>
            {
                var answers = studentAnswersGrouped.Where(sa => sa.UserName == student.UserName);
                body = answers.Select(sa => new object[]
                {
                    sa.Course,
                    sa.Session,
                    sa.Stage,
                    Locale.ResourceManager.GetString($"Export_StudentResults_Level_{(int)sa.Level}"),
                    sa.Grade * 10
                })
                .ToArray();

                excelPackageBuilder
                    .WithWorkSheet(student.UserName, ResultsHeaders, body);
            });

            return excelPackageBuilder.Build().GetAsByteArray();
        }

        private async Task<IEnumerable<StudentResult>> GetStudentsResults(
            Group group,
            IEnumerable<StudentUserApiDTO> students,
            IEnumerable<StudentProgress> studentsProgress,
            IEnumerable<StudentAnswer> studentsAnswers)
        {
            var middleGrades = await _trackingService.GetStudentsListSubjectsAverage(group.Id);             

            return group.Students
                .Where(s => students.Any(st => st.UserName == s.UserName))
                .Select(studentGroup =>
                {
                    var student = students
                        .FirstOrDefault(s => studentGroup.UserName == s.UserName);
                    var progress = studentsProgress
                        .FirstOrDefault(p => p.UserName == studentGroup.UserName);
                    var studentAnswers = studentsAnswers
                        .Where(s => s.UserName == studentGroup.UserName);

                    return new StudentResult()
                    {
                        FullName = $"{student.FirstSurname} {student.SecondSurname}, {student.Name}",
                        UserName = student.UserName,
                        CompletedSessions = group.SubjectKey == SubjectKey.Emat
                            ? StudentAnswerCalculation.CalculateCompletedEmatSessions(studentAnswers)
                            : StudentAnswerCalculation.CalculateCompletedLudiSessions(studentAnswers),
                        AverageScore = decimal.Round((decimal)middleGrades.studentsList.Where(b => b.userName == student.UserName).Select(av => av.SuperBlocksAverage).First(),1),
                        CurrentSession = progress != null ? progress.Session : 1,
                        CurrentCourse = progress != null ? progress.Course : studentGroup.Group.Course
                    };
                })
                .OrderBy(g => g.FullName);
        }
    }
}
