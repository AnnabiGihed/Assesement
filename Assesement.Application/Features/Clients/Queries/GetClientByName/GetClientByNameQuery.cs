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

		public GetClientByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<GetClientByNameDto>> Handle(GetClientByNameQuery query, CancellationToken cancellationToken)
		{
			#region Check if Client Exist
			var Client = await _unitOfWork.Repository<Client>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.ClientName.ToLower());

			if (Client == null)
				return await Result<GetClientByNameDto>.FailureAsync("Client Not Found.");
			#endregion

			var ClientDto = _mapper.Map<GetClientByNameDto>(Client);

			return await Result<GetClientByNameDto>.SuccessAsync(ClientDto, "Client Found.");
		}
	}
}
