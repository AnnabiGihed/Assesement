using System.Linq.Expressions;
using Assessment.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Assessment.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Query;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
	{
		private readonly ApplicationDbContext _dbContext;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IQueryable<T> Entities => _dbContext.Set<T>();
		public Task UpdateAsync(T entity)
		{
			_dbContext.Update(entity);
			return Task.CompletedTask;
		}
		public Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			return Task.CompletedTask;
		}
		public async Task<T> AddAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			return entity;
		}
		public async Task<List<T>> GetAllAsync()
		{
			return await _dbContext
				.Set<T>()
				.ToListAsync();
		}
		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}
		public Task UpdateRangeAsync(IList<T> entities)
		{
			_dbContext.UpdateRange(entities);
			return Task.CompletedTask;
		}
		public async Task<IList<T>> AddRangeAsync(IList<T> entities)
		{
			await _dbContext.Set<T>().AddRangeAsync(entities);
			return entities;
		}
		public async Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression)
		{
			return await _dbContext.Set<T>().Where(expression).ToListAsync();
		}
		public Task<List<T>> GetWithInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
		{
			var result = _dbContext.Set<T>().AsQueryable();

			if (include != null)
				result = include(result);

			return result.ToListAsync();
		}
	}
}
