using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
{
	public class WholesalerSaleRepository : IWholesalerSaleRepository
	{
		private readonly IGenericRepository<WholesalerSale> _repository;

		public WholesalerSaleRepository(IGenericRepository<WholesalerSale> repository)
		{
			_repository = repository;
		}
	}
}
