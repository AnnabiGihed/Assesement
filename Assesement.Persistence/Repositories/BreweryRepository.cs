using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
{
	public class BreweryRepository : IBreweryRepository
	{
		private readonly IGenericRepository<Brewery> _repository;

		public BreweryRepository(IGenericRepository<Brewery> repository)
		{
			_repository = repository;
		}
	}
}
