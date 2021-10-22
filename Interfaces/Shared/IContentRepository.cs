using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Entities.Content;

namespace Api.Interfaces.Shared
{	
	public interface IContentRepository<T> where T : BaseEntity
	{ 
		Task<T> Get(Guid id);
		Task<List<T>> GetAll();
		Task<List<T>> Find(Expression<Func<T, bool>> predicate, string[] includes = null);
		Task<T> FindSingle(Expression<Func<T, bool>> predicate, string[] includes = null);
		Task<bool> Any(Expression<Func<T, bool>> predicate);
		IQueryable<T> Query(string[] includes = null);
		Task<T> Add(T entity);
		Task<List<T>> Add(List<T> entities);
		Task Update(T entity);
		Task Update(List<T> entities);
		Task Delete(Guid id);
		Task Delete(T entity);
		Task Delete(List<T> entities);
	}
}