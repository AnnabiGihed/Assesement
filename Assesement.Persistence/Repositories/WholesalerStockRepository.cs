using Assessment.Domain.Entities;
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
	}
}
