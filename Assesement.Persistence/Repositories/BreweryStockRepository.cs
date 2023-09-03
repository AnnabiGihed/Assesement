using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
{
	public class BreweryStockRepository : IBreweryStockRepository
	{
		private readonly IGenericRepository<BreweryStock> _repository;

		public BreweryStockRepository(IGenericRepository<BreweryStock> repository)
		{
			_repository = repository;
		}

		public async Task<BreweryStock> GetBreweryStockByBeer(int breweryId, int beerId)
		{
			return await _repository.Entities.FirstOrDefaultAsync(x => x.BeerId == beerId && x.BreweryId == breweryId);
		}
	}
}
