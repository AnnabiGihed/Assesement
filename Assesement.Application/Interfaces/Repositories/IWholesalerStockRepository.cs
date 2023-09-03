using Assessment.Domain.Entities;

namespace Assessment.Application.Interfaces.Repositories
{
	public interface IWholesalerStockRepository
	{
		Task<WholesalerStock> GetWholesalerStocksByBeer(int wholesalerId, int beerId);
		Task<WholesalerStock> GetWholesalerStocksByBeerWithInclude(int wholesalerId, int beerId);
	}
}
