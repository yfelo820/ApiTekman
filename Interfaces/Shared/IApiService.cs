using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Api.Interfaces.Shared
{
	public interface IApiService<T>
	{
		Task<T> GetSingle(Guid id);
		Task<List<T>> GetAll();
		Task<T> Add(T entity);
		Task<T> Update(T entity);
		Task<T> Delete(Guid id);
	}
}