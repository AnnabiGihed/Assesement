using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
{
	public class BreweryStockRepository : IBreweryStockRepository
	{
		private readonly IGenericRepository<BreweryStock> _repository;

		public BreweryStockRepository(IGenericRepository<BreweryStock> repository)
		{
			_repository = repository;
		}
	}
}
