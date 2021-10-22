using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTO.Teachers;

namespace Api.Interfaces.Parents
{
    public interface ITrackingService
    {
        Task<List<StudentTrackingDTO>> GetMultiples(string subjectKey);
        Task<StudentTrackingExtendedDTO> GetSingleStudentDetail(string userName, string subjectKey);
    }
}
