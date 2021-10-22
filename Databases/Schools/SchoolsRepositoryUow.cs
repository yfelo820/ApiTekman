using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api.Databases.Schools
{
    public class SchoolsRepositoryUow<T> : ISchoolsRepositoryUow<T> where T : BaseEntity
    {
        private SchoolsDbContext _db;
        public SchoolsRepositoryUow(SchoolsDbContext dbContext) => _db = dbContext;

        public Task<T> Add(T entity)
		{
			_db.Set<T>().Add(entity);
            return Task.FromResult(entity); 
		}

		public Task<List<T>> Add(List<T> entities)
		{
			_db.Set<T>().AddRange(entities);
            return Task.FromResult(entities); 
		}

		public Task Delete(T entity)
		{
			_db.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

		public Task Delete(List<T> entities) {
			_db.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public DbSet<T> EntityDbSet() => _db.Set<T>();

        public Task Update(T entity)
		{
			_db.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
		public Task Update(List<T> entities)
		{
			foreach(var entity in entities) _db.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

		public async Task<List<T>> Find(Expression<Func<T, bool>> predicate, string[] includes = null)
		{
            return await Query(includes)
                            .Where(predicate)
                            .ToListAsync();
		}

		public async Task<List<T>> GetAll()
		{
			return await _db.Set<T>().ToListAsync();
		}

		public async Task<T> Get(Guid id)
		{
			return await _db.Set<T>().FindAsync(id);
		}

        public IQueryable<T> Query(string[] includes = null)
		{
			includes = includes ?? new string[0];
			return includes.Aggregate(
				_db.Set<T>().AsQueryable(),
				(current, include) => current.Include(include)
			);
		}

		public async Task<T> FindSingle(Expression<Func<T, bool>> predicate, string[] includes = null)
		{
			return await Query(includes)
                            .Where(predicate)
                            .FirstOrDefaultAsync();
		}
		
		public async Task<bool> Any(Expression<Func<T, bool>> predicate)
		{
			return await _db.Set<T>().AnyAsync(predicate);
		}
    }
}