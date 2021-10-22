using System.Threading.Tasks;

namespace Api.Interfaces.Shared
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
    }
}