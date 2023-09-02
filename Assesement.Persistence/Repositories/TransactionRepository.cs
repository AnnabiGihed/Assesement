using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly IGenericRepository<Transaction> _repository;

		public TransactionRepository(IGenericRepository<Transaction> repository)
		{
			_repository = repository;
		}
	}
}
