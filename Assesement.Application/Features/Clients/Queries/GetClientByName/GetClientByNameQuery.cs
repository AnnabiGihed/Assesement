using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Clients.Queries.GetClientByName
{
	public record GetClientByNameQuery : IRequest<Result<GetClientByNameDto>>
	{
		public string ClientName { get; set; }
	}

	internal class GetClientByNameQueryHandler : IRequestHandler<GetClientByNameQuery, Result<GetClientByNameDto>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IClientRepository _clientRepository;

		public GetClientByNameQueryHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_clientRepository = clientRepository;
		}

		public async Task<Result<GetClientByNameDto>> Handle(GetClientByNameQuery query, CancellationToken cancellationToken)
		{
			var Client = await _unitOfWork.Repository<Client>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.ClientName.ToLower());

			if (Client == null)
				return await Result<GetClientByNameDto>.FailureAsync("Client Not Found.");

			var ClientDto = _mapper.Map<GetClientByNameDto>(Client);

			return await Result<GetClientByNameDto>.SuccessAsync(ClientDto, "Client Found.");
		}
	}
}
