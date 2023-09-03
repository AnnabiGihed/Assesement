﻿using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
{
	public class WholesalerSaleRepository : IWholesalerSaleRepository
	{
		private readonly IGenericRepository<WholesalerSale> _repository;

		public WholesalerSaleRepository(IGenericRepository<WholesalerSale> repository)
		{
			_repository = repository;
		}

		public async Task<int> GetLastUsedIndex()
		{
			return await _repository.Entities.MaxAsync(x => x.Id);
		}
	}
}
