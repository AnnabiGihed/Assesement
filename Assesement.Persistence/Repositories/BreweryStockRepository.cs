using Assessment.Domain.Entities;
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
	}
}
