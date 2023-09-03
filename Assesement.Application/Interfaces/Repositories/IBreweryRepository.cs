using Assessment.Domain.Entities;

namespace Assessment.Application.Interfaces.Repositories
{
	public interface IBreweryRepository
	{
		Task<Brewery> GetBreweryByNameAsync(string name);
		Task<List<Brewery>> GetBreweriesWithThereStocksAndBeers();
	}
}
