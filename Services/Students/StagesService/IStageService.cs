using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Students;

namespace Api.Services.Students
{
    public interface IStageService
    {
        Task<StageActivityDTO> GetNext(int course, int session, int stage);
        Task<IEnumerable<StageActivityDTO>> GetAll(int course, int session);
    }

    public interface IEmatStageService : IStageService { }
    public interface IEmatInfantilStageService : IStageService { }
    public interface ILudiStageService : IStageService { }
    public interface ISuperletrasStageService : IStageService { }
}
