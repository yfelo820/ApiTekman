using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.DTO.Teachers;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;
using Api.Resources;

namespace Api.Services.Teachers
{
    public class StudentInfoExport : IStudentInfoExport
    {
        private readonly GroupsService _groupsService;
        private readonly IUserService _userService;
        private readonly IClaimsService _claimsService;

        private static string[] Headers => new string[] {
            Locale.Export_StudentsInfo_GroupName,
            Locale.Export_StudentsInfo_GroupKey,
            Locale.Export_StudentsInfo_StudentName,
            Locale.Export_StudentsInfo_User,
            Locale.Export_StudentsInfo_Password
        };

        public StudentInfoExport(
            IApiService<GroupDTO> groupsService,
            IUserService userService,
            IClaimsService claimsService)
        {
            _groupsService = groupsService as GroupsService;
            _userService = userService;
            _claimsService = claimsService;
        }

        public async Task<byte[]> Export(Guid groupId)
        {
            var language = _claimsService.GetLanguageKey();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            var workSheetTitle = DateTime.Now.ToString("ddMMyyyy");
            var studentsInformation = await GetStudentsInfo(groupId);
            var body = studentsInformation.Students
                .Select(s => new object[]
                {
                    studentsInformation.Name,
                    studentsInformation.Key,
                    s.Name,
                    s.Username,
                    s.Password
                }).ToArray();

            var excelPackage = new ExcelPackageBuilder()
                .WithTitle(Locale.Export_StudentsInfo_Title)
                .WithWorkSheet(workSheetTitle, Headers, body)
                .Build();

            return excelPackage.GetAsByteArray();
        }

        

        private async Task<ExcelGroupDTO> GetStudentsInfo(Guid groupId)
        {
            var group = await _groupsService.GetSingleGroup(groupId);
            var usernames = group.Students.Select(s => s.UserName).ToList();
            var studentsList = await _userService.GetAllStudents();
            return new ExcelGroupDTO()
            {
                Name = group.Name,
                Key = group.Key,
                Students = studentsList
                .Where(s => usernames.Contains(s.UserName))
                .Select(s => new ExcelStudentDTO()
                {
                    Username = s.UserName,
                    Name = s.Name + " " + s.FirstSurname,
                    Password = group.Students
                        .Where(st => s.UserName == st.UserName)
                        .Select(st => st.AccessNumber)
                        .FirstOrDefault()
                }).ToList(),
            };
        }
    }
}