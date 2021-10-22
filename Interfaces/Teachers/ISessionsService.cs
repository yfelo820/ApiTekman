using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Teachers;

namespace Api.Interfaces.Teachers
{
    public interface ISessionsService
    {
        Task<SessionDTO> Get(string subject);
        Task<IEnumerable<CourseSessionsDTO>> GetByCourse(string subjectKey);
    }
}