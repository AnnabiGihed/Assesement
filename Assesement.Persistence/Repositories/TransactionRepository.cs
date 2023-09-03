using Assessment.Domain.Entities;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
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
