using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
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
