using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
{
	public class WholesalerRepository : IWholesalerRepository
	{
		private readonly IGenericRepository<Wholesaler> _repository;

		public WholesalerRepository(IGenericRepository<Wholesaler> repository)
		{
			_repository = repository;
		}
	}
}
