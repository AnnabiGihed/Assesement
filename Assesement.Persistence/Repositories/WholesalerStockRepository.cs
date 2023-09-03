using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
{
	public class WholesalerStockRepository : IWholesalerStockRepository
	{
		private readonly IGenericRepository<WholesalerStock> _repository;

		public WholesalerStockRepository(IGenericRepository<WholesalerStock> repository)
		{
			_repository = repository;
		}

		public async Task<WholesalerStock> GetWholesalerStockByBeer(int wholesalerId, int beerId)
		{
			return await _repository.Entities.FirstOrDefaultAsync(x => x.BeerId == beerId && x.WholesalerId == wholesalerId);
		}
	}
}
