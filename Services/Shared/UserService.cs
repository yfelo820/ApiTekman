using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Shared;
using Api.DTO.Teachers;
using Api.Helpers;
using Api.Interfaces.Shared;
using Api.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Api.Services.Shared
{
    public class UserService : IUserService
    {
        private const string DefaultStudentPassword = "DefaultPassword-";
        private const string SuperletrasCatLicenseSubject = "superlletres";
        private readonly HttpJsonClient _json;
        private readonly HttpFormClient _form;
        private readonly IDictionary<string, string> _cultureLanguages;
        private readonly IClaimsService _claimsService;

        public UserService(
            IOptions<CultureLanguagesSettings> options,
            IClaimsService claimsService,
            IConfiguration config
        )
        {
            _form = new HttpFormClient(config["UserApiUrl"]);
            _json = new HttpJsonClient(config["UserApiUrl"]);
            var token = claimsService.GetAccessToken().Result;
            _json.SetAuth(token);
            _cultureLanguages = options.Value;
            _claimsService = claimsService;
        }

        public async Task<IEnumerable<StudentUserApiDTO>> GetAllStudents(string stage = Stage.Primary)
        {
            var students =
                await _json.Get<List<StudentUserApiDTO>>($"student/v2/students?IsActive=true&stage={stage.ToLower()}");
            // Fix userapi returning null second surname, inactive users & not primaria users
            students = students
                .Select(s =>
                {
                    if (string.IsNullOrEmpty(s.SecondSurname)) s.SecondSurname = "";
                    return s;
                }).ToList();
            return students;
        }

        public async Task<IEnumerable<StudentUserApiDTO>> GetAllStudentsByStage(string stage = Stage.Primary)
        {
            var accessToken = await _claimsService.GetAccessToken();
            _json.SetAuth(accessToken);
            return await GetAllStudents(stage);
        }

        public async Task<IEnumerable<string>> GetLicensedLanguages(string subject)
        {
            var licenseSubject = GetLicenceSubject(subject);
            var licenses =
                await _json.Get<LicensesUserApiDTO>($"v4/school/licenses-v3-format?program={licenseSubject}");
            var stagesAccordingSubject = GetStages(subject);
            var languages = licenses.licencias
                .SelectMany(l => l.cursos)
                .Where(c => stagesAccordingSubject.Contains(c.etapa) && c.licencias > 0 &&
                            _cultureLanguages.ContainsKey(c.idioma))
                .Select(c => _cultureLanguages[c.idioma])
                .Distinct();

            return FilterSuperletrasLanguages(subject, languages);
        }

        private static IEnumerable<string> FilterSuperletrasLanguages(string subject, IEnumerable<string> languages)
        {
            if (subject == SubjectKey.SuperletrasCat)
            {
                return languages.Where(l => l == Culture.Cat);
            }

            if (subject == SubjectKey.Superletras)
            {
                return languages.Any(l => l == Culture.Es || l == Culture.Mx)
                    ? new List<string>{ Culture.Es }
                    : new List<string>();
            }

            return languages;
        }

        public async Task<IEnumerable<string>> GetLicensesLanguagesBySubject(string subject)
        {
            var accessToken = await _claimsService.GetAccessToken();
            _json.SetAuth(accessToken);
            return await GetLicensedLanguages(subject);
        }

        public void Authenticate(string token)
        {
            _json.SetAuth(token);
        }

        public async Task<UserApiLoginDTO> LoginStudent(string username, string subjectKey)
        {
            var loginPayload = new
            {
                username,
                password = DefaultStudentPassword + username,
                grant_type = "password",
                application = subjectKey == SubjectKey.Emat
                    ? Application.Emat
                    : Application.Ludi
            };

            var loginInfo = await _form.Post<UserApiLoginDTO>("student/v1/Token", loginPayload);
            return loginInfo;
        }

        public async Task<StudentUserApiDTO> GetSingleStudent(string username)
        {
            return await _json.Get<StudentUserApiDTO>("student/v2/students?Username=" + username);
        }

        public async Task<string> RegisterStudentImpersonation(string userName)
        {
            var impersonationPayload = new
            {
                userEmail = _claimsService.GetUserName(),
                studentUsername = userName
            };
            var impersonationInfo = await _json.Post<ImpersonationDTO>("v5/impersonalisation", impersonationPayload);
            
            return impersonationInfo.Hash;
        }

        private static string[] GetStages(string subject)
        {
            //TODO: normalize stages names in TKcore between different regions to avoid this
            return subject == SubjectKey.EmatInfantil
                ? new[] {Stage.Infantil, Stage.Preescolar, Stage.Parvulario}
                : new[] {Stage.Primary};
        }

        private string GetLicenceSubject(string subject)
        {
            switch (subject)
            {
                case SubjectKey.EmatInfantil:
                case SubjectKey.EmatMx:
                case SubjectKey.EmatCat:
                    return SubjectKey.Emat;
                case SubjectKey.Superletras:
                    return SubjectKey.Superletras;
                case SubjectKey.SuperletrasCat:
                    return SuperletrasCatLicenseSubject;
                default:
                    return subject;
            }
        }
    }
}