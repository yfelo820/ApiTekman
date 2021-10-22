using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Shared;

namespace Api.Interfaces.Shared
{
    public interface ISsoService
    {
        Task<TokenResponseDto> GetToken(string code);
        Task<TokenResponseDto> GetStudentToken(string code);
        Task<SsoClaimsDTO> GetClaims(TokenResponseDto token);
        IEnumerable<string> GetClientIds();
        ServiceProvider GetServiceProvider();
        ServiceProvider GetStudentServiceProvider();
    }
}