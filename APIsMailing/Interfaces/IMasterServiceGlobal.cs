using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.APIsMailing.Responses;

namespace Api.APIsMailing.Interfaces
{
    public interface IMasterServiceGlobal
    {
        Task<MasterResponseGlobal> GetMasterResponse(Guid groupId);
        Task<List<GroupDto>> GetAllEmatGroupDetails();
    }
}
