using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Breweries.Queries.GetAllBreweriesAndTheirBeers
{
	public record GetAllBreweriesAndTheirBeersQuery : IRequest<Result<List<GetAllBreweriesAndTheirBeersDto>>>;

	internal class GetAllBreweriesAndTheirBeersQueryHandler : IRequestHandler<GetAllBreweriesAndTheirBeersQuery, Result<List<GetAllBreweriesAndTheirBeersDto>>>
	{
		private readonly IMapper _mapper;
		private readonly IBreweryRepository _breweryRepository;

		public GetAllBreweriesAndTheirBeersQueryHandler(IBreweryRepository breweryRepository, IMapper mapper)
		{
			_mapper = mapper;
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
