using Assessment.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace Assessment.Application.Interfaces.Repositories
{
	public interface IGenericRepository<T> where T : class, IEntity
	{
		IQueryable<T> Entities { get; }
		Task<T> GetByIdAsync(int id);
		Task<List<T>> GetAllAsync();
		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task<List<T>> GetWithInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
	}
}
