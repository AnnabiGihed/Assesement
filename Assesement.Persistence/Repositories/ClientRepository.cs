using Assessment.Domain.Entities;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Persistence.Repositories
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
