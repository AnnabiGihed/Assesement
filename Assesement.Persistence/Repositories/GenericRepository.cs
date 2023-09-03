﻿using Assessment.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Assessment.Persistence.Contexts;
using Assessment.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Query;

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

		public async Task<T> AddAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			return entity;
		}

		public Task UpdateAsync(T entity)
		{
			T exist = _dbContext.Set<T>().Find(entity.Id);
			_dbContext.Entry(exist).CurrentValues.SetValues(entity);
			return Task.CompletedTask;
		}

		public Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			return Task.CompletedTask;
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

		public Task<List<T>> GetWithInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
		{
			var result = _dbContext.Set<T>().AsQueryable();

			if (include != null)
				result = include(result);

			return result.ToListAsync();
		}
	}
}
