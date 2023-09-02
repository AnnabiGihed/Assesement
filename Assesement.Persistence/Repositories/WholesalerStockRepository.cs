using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
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
