using System;
using System.Threading.Tasks;
using Api.APIsMailing.Responses;

namespace Api.APIsMailing.Interfaces
{
    public interface IMasterServiceDetail
    {
        Task<MasterResponseDetail> GetMasterResponse(Guid groupId, string username, int session);
    }
}
