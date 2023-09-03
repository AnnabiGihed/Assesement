using Assessment.Domain.Entities;

namespace Assessment.Application.Interfaces.Repositories
{
	public interface IBreweryStockRepository
	{
		Task<BreweryStock> GetBreweryStockByBeer(int breweryId, int beerId);
	}
}
