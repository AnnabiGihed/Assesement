using Assessment.Domain.Entities;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
{
	public class BeerRepository : IBeerRepository
	{
		private readonly IGenericRepository<Beer> _repository;

		public BeerRepository(IGenericRepository<Beer> repository)
		{
			_repository = repository;
		}
	}
}
