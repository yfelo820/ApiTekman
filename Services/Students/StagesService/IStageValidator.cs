using System.Threading.Tasks;

namespace Api.Services.Students.StagesService
{
    public interface IStageValidator
    {
        Task Validate(string userName, string subject, string language, int course, int session);
    }
}
