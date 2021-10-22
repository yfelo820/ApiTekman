using Api.Constants;
using Api.Exceptions;

namespace Api.Services.Students
{
    public class StageServiceFactory : IStageServiceFactory
    {
        private readonly IEmatStageService _ematStageService;
        private readonly IEmatInfantilStageService _ematInfantilStageService;
        private readonly ILudiStageService _ludiStageService;
        private readonly ISuperletrasStageService _superletrasStageService;

        public StageServiceFactory(IEmatStageService ematStageService,
            IEmatInfantilStageService ematInfantilStageService,
            ILudiStageService ludiStageService,
            ISuperletrasStageService superletrasStageService)
        {
            _ematStageService = ematStageService;
            _ematInfantilStageService = ematInfantilStageService;
            _ludiStageService = ludiStageService;
            _superletrasStageService = superletrasStageService;
        }

        public IStageService Create(string subject)
        {
            switch (subject)
            {
                case SubjectKey.Emat:
                    return _ematStageService;
                case SubjectKey.EmatInfantil:
                    return _ematInfantilStageService;
                case SubjectKey.Ludi:
                case SubjectKey.LudiCat:
                    return _ludiStageService;
                case SubjectKey.Superletras:
                case SubjectKey.SuperletrasCat:
                    return _superletrasStageService;
                default:
                    throw new InvalidSubjectException($"The subject ({subject}) doesn't exist.");
            }
        }
    }
}
