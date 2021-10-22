using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTO.Parents;
using Api.Entities.Schools;
using Api.Helpers;
using Api.Interfaces.Parents;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Services.Parents
{
    public class StudentsService : IStudentsService
    {
        private readonly HttpJsonClient _json;

        ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly IClaimsService _claims;
        private readonly IConfiguration _config;

        public StudentsService(
            ISchoolsRepository<StudentGroup> studentGroups,
            IClaimsService claims,
            IConfiguration config
        )
        {
            _json = new HttpJsonClient(config["UserApiUrl"]);
            var accessToken = claims.GetAccessToken().Result;
            _json.SetAuth(accessToken);
            _studentGroups = studentGroups;
            _claims = claims;
            _config = config;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudents(string subjectKey)
        {
            var email = _claims.GetUserName();
            return await GetAllStudents(email, _json, subjectKey);
        }

        //Called from AuthService before the parent user claims have been set
        public async Task<IEnumerable<string>> GetAllStudentLanguages(string email, string token, string subjectKey)
        {
            var jsonClient = new HttpJsonClient(_config["UserApiUrl"]);
            jsonClient.SetAuth(token);
            return await GetAllStudentLanguages(email, jsonClient, subjectKey);
        }

        public async Task<StudentDTO> GetStudent(string userName, string subjectKey)
        {
            var email = _claims.GetUserName();
            var studentList = await GetUsersFromTkCore(email, _json);
            var student = studentList.Single(s => s.UserName == userName);
            var studentGroup = await _studentGroups
                .Query(new[] { "Group" })
                .SingleOrDefaultAsync(g => g.UserName == userName && g.Group.SubjectKey == subjectKey);

            if (studentGroup != null)
            {
                student.AccessNumber = studentGroup.AccessNumber;
                student.GroupKey = studentGroup.Group.Key;
                student.SubjectKey = studentGroup.Group.SubjectKey;
                student.LanguageKey = studentGroup.Group.LanguageKey;
            }
            if (string.IsNullOrEmpty(student.SecondSurname)) student.SecondSurname = "";

            return new StudentDTO(student);
        }

        private async Task<IList<string>> GetUserNamesFromTkCore(string email, HttpJsonClient httpJsonClient)
        {
            return (await GetUsersFromTkCore(email, httpJsonClient)).Select(s => s.UserName).ToList();
        }

        private async Task<IList<StudentUserApiDTO>> GetUsersFromTkCore(string email, HttpJsonClient httpJsonClient)
        {
            var response = await httpJsonClient.Get<StudentsUserApiDTO>($"v5/parents/{email}/students");
            return response.Students.Where(s => s.Status == "active").ToList();
        }
        
        private async Task<IEnumerable<StudentDTO>> GetAllStudents(string email, HttpJsonClient jsonClient, string subjectKey)
        {
            var studentList = await GetUsersFromTkCore(email, jsonClient);
            var userNamesList = studentList.Select(s => s.UserName).ToList();
            
            var studentGroups = await _studentGroups
                .Query(new[] { "Group" })
                .Where(g => userNamesList.Contains(g.UserName) && g.Group.SubjectKey == subjectKey)
                .ToListAsync();

            List<StudentUserApiDTO> students = studentList
                .Select(s =>
                {
                    var studentGroup = studentGroups.SingleOrDefault(g => g.UserName == s.UserName);

                    if (studentGroup != null)
                    {
                        s.AccessNumber = studentGroup.AccessNumber;
                        s.GroupKey = studentGroup.Group.Key;
                        s.SubjectKey = studentGroup.Group.SubjectKey;
                        s.LanguageKey = studentGroup.Group.LanguageKey;
                    }
                    if (string.IsNullOrEmpty(s.SecondSurname)) s.SecondSurname = "";
                    return s;
                }).ToList();

            return students.ConvertAll(s => new StudentDTO(s));
        }

        private async Task<IEnumerable<string>> GetAllStudentLanguages(string email, HttpJsonClient jsonClient,
            string subjectKey)
        {
            var userNamesList = await GetUserNamesFromTkCore(email, jsonClient);
            
            var studentGroups = await _studentGroups
                .Query(new[] { "Group" })
                .Where(g => userNamesList.Contains(g.UserName) && g.Group.SubjectKey == subjectKey)
                .ToListAsync();

            IEnumerable<string> languages = studentGroups
                .Select(s => s.Group.LanguageKey)
                .Distinct()
                .ToList();

            return languages;
        }
    }
}
