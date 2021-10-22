using Api.Constants;

namespace Api.Interfaces.Shared
{
    public interface IHttpContextService
    {
        string GetSubjectFromUri();
        string GetRedirectUri();
        string GetStudentRedirectUri();
        string GetServerUri();
        Region GetEmatRegionFromUri();
        Region GetSupercibersRegionFromUri();
    }
}
