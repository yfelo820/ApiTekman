using Api.Interfaces.Students;

namespace Api.Services.Students
{
    public interface IStageServiceFactory
    {
        IStageService Create(string subject);
    }
}
