using Assesement.Domain.Entities;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private readonly IGenericRepository<Client> _repository;

		public ClientRepository(IGenericRepository<Client> repository)
		{
			_repository = repository;
		}
	}
}
