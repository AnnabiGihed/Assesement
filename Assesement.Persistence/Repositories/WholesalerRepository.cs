using Assessment.Domain.Entities;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
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
