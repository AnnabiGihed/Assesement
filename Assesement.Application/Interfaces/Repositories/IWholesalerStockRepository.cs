using Assessment.Domain.Entities;

namespace Assessment.Application.Interfaces.Repositories
{
	public interface IWholesalerStockRepository
	{
		Task<WholesalerStock> GetWholesalerStockByBeer(int wholesalerId, int beerId);
	}
}
