using Assessment.Domain.Entities;

namespace Assessment.Application.Interfaces.Repositories
{
	public interface IBreweryRepository
	{
		Task<List<Brewery>> GetBreweriesWithThereStocksAndBeers();
	}
}
