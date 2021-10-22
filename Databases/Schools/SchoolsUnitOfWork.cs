using System.Threading.Tasks;
using Api.Interfaces.Shared;

namespace Api.Databases.Schools
{
    public class SchoolsUnitOfWork : IUnitOfWork
    {
        private readonly SchoolsDbContext _db;

        public SchoolsUnitOfWork(SchoolsDbContext db)
        {
            _db = db;
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}