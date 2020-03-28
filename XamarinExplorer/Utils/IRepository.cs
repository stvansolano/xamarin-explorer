using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
	public interface IRepository<T>
		where T : class
	{
		//Task<bool> AddAsync(T item);
		//Task<bool> UpdateAsync(object id, T item);
		//Task<bool> DeleteAsync(object id);
		//Task<T> GetAsync(string id);
		Task<IEnumerable<T>> GetAsync(bool forceRefresh = false);
	}
}
