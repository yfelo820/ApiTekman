using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Teachers
{
    public class StudentsService : IStudentsService
    {
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly IUserService _userService;
        private readonly IClaimsService _claims;
        private readonly IHttpContextService _httpContextService;

        public StudentsService(
            ISchoolsRepository<StudentGroup> studentGroups,
            IUserService userService,
            IClaimsService claims,
            IHttpContextService httpContextService)
        {
            _studentGroups = studentGroups;
            _userService = userService;
            _claims = claims;
            _httpContextService = httpContextService;
        }

        public async Task<List<StudentDTO>> GetAll()
        {
            var subjectKey = _httpContextService.GetSubjectFromUri();
            var languageKey = _claims.GetLanguageKey();
            var students = subjectKey.Equals(SubjectKey.EmatInfantil) ? 
                await _userService.GetAllStudentsByStage(Stage.Infantil) : 
                await _userService.GetAllStudentsByStage();
            var studentNames = students.Select(s => s.UserName).ToList();
            var groups = await _studentGroups
                .Find(g =>
                    studentNames.Contains(g.UserName)
                    && g.Group.SubjectKey == subjectKey
                    && g.Group.LanguageKey == languageKey,
                    new[] { "Group" }
                );
            foreach (var group in groups) group.Group.Students = null; // removing self reference
            return students.ToList().ConvertAll(
                s => new StudentDTO(s, groups.FirstOrDefault(sg => sg.UserName == s.UserName))
            )
            .OrderBy(s => s.FirstSurname)
            .ThenBy(s => s.SecondSurname)
            .ThenBy(s => s.Name)
            .ToList();
        }

        public async Task<StudentDTO> GetSingle(string userName)
        {
            var subjectKey = _claims.GetSubjectKey();
            var languageKey = _claims.GetLanguageKey();
            var student = await _userService.GetSingleStudent(userName);
            var studentGroup = await _studentGroups
                .Query(new[] { "Group" })
                .Where(g =>
                    g.UserName == userName
                    && g.Group.SubjectKey == subjectKey
                    && g.Group.LanguageKey == languageKey
                )
                .FirstOrDefaultAsync();
            return new StudentDTO(student, studentGroup);
        }

        public async Task<List<StudentDTO>> GetMultiples(List<string> userNames)
        {

            var subjectKey = _claims.GetSubjectKey();
            var languageKey = _claims.GetLanguageKey();
            var students = subjectKey.Equals(SubjectKey.EmatInfantil) ? await _userService.GetAllStudents(Stage.Infantil) : await _userService.GetAllStudents();
            var studentsList = students.Where(s => userNames.Any(u => u == s.UserName)).ToList();
            var studentNames = studentsList.Select(s => s.UserName).ToList();
            var groups = await _studentGroups
                .Find(g =>
                        studentNames.Contains(g.UserName)
                        && g.Group.SubjectKey == subjectKey
                        && g.Group.LanguageKey == languageKey,
                    new[] { "Group" }
                );
            foreach (var group in groups) group.Group.Students = null; // removing self reference
            return studentsList.ConvertAll(
                    s => new StudentDTO(s, groups.FirstOrDefault(sg => sg.UserName == s.UserName))
                )
                .OrderBy(s => s.FirstSurname)
                .ThenBy(s => s.SecondSurname)
                .ThenBy(s => s.Name)
                .ToList();
        }
    }
}
