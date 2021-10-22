using Api.Constants;
using Api.Interfaces.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Api.Services.Shared
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHostingEnvironment _env;

        public HttpContextService(IHttpContextAccessor contextAccessor, IHostingEnvironment env)
        {
            _contextAccessor = contextAccessor;
            _env = env;
        }

        public string GetSubjectFromUri()
        {
            var uri = _contextAccessor.HttpContext.Request.Host.ToString();

            if (uri.Contains(Domain.CiberEmatInfantil)) return SubjectKey.EmatInfantil;
            if (uri.Contains(Domain.CiberEmat)) return SubjectKey.Emat;
            if (uri.Contains(Domain.CiberLudiletras)) return SubjectKey.Ludi;
            if (uri.Contains(Domain.CiberEmatUniversal)) return SubjectKey.Emat;
            if (uri.Contains(Domain.SuperCiberCat)) return SubjectKey.SuperletrasCat;
            if (uri.Contains(Domain.SuperCiber)) return SubjectKey.Superletras;
            
            return SubjectKey.LudiCat;
        }

        public Region GetEmatRegionFromUri()
        {
            var uri = _contextAccessor.HttpContext.Request.Host.ToString();

            if (uri.Contains(Domain.CiberEmatCat)) return Region.Catalonia;
            if (uri.Contains(Domain.CiberEmatMX)) return Region.Mexico;

            return Region.Spain;
        }

        public Region GetSupercibersRegionFromUri()
        {
            var uri = _contextAccessor.HttpContext.Request.Host.ToString();

            return uri.Contains(Domain.SuperCiberCat) ? Region.Catalonia : Region.Spain;
        }

        public string GetRedirectUri()
        {
            var teachersPath = GetSubjectFromUri() == SubjectKey.EmatInfantil ? "/teachers-infantil/" : "/teachers/";
            return GetServerUri() + teachersPath + "callback";
        }

        public string GetStudentRedirectUri()
        {
            return GetServerUri() + "/students/login/callback";
        }

        public string GetServerUri()
        {
            var protocol = _env.IsDevelopment() ? "http://" : "https://";
            var host = _contextAccessor.HttpContext.Request.Host.ToString().Replace("www.", "");
            return protocol + host;
        }
    }
}
