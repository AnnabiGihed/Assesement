using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Breweries.Queries.GetAllBreweriesAndTheirBeers
{
	public record GetAllBreweriesAndTheirBeersQuery : IRequest<Result<List<GetAllBreweriesAndTheirBeersDto>>>;

	internal class GetAllBreweriesAndTheirBeersQueryHandler : IRequestHandler<GetAllBreweriesAndTheirBeersQuery, Result<List<GetAllBreweriesAndTheirBeersDto>>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBreweryRepository _breweryRepository;

		public GetAllBreweriesAndTheirBeersQueryHandler(IUnitOfWork unitOfWork, IBreweryRepository breweryRepository, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_breweryRepository = breweryRepository;
		}

		public async Task<Result<List<GetAllBreweriesAndTheirBeersDto>>> Handle(GetAllBreweriesAndTheirBeersQuery query, CancellationToken cancellationToken)
		{
			var Breweries = await _breweryRepository.GetBreweriesWithThereStocksAndBeers();
			var BreweriesDtos = _mapper.Map<List<GetAllBreweriesAndTheirBeersDto>>(Breweries);

			return await Result<List<GetAllBreweriesAndTheirBeersDto>>.SuccessAsync(BreweriesDtos);
		}
	}
}
