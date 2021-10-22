using System.Threading.Tasks;

namespace Api.Interfaces.Shared
{
    public interface IClaimsService
    {
        string GetName();
        string GetSurName();
        string GetSchoolId();
        string GetUserName ();
        string GetAvailableLanguages ();
        Task<string> GetAccessToken();
        string GetSubjectKey(); 
        string GetLanguageKey();
    }
}