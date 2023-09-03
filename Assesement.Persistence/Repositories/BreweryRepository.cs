using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
{
	public class BreweryRepository : IBreweryRepository
	{
		private readonly IGenericRepository<Brewery> _repository;

		public BreweryRepository(IGenericRepository<Brewery> repository)
		{
			_repository = repository;
		}

		public async Task<List<Brewery>> GetBreweriesWithThereStocksAndBeers()
		{
			return await _repository.GetWithInclude(b => b
				.Include(br => br.Stocks)
					.ThenInclude(s => s.Beer));
		}

		public async Task<Brewery> GetBreweryByNameAsync(string name)
		{
			return await _repository.Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
		}
	}
}
