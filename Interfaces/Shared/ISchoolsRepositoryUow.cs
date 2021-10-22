using Api.Entities.Schools;

namespace Api.Interfaces.Shared
{
    public interface ISchoolsRepositoryUow<T> : ISchoolsRepository<T> where T : BaseEntity
    {
    }
}