using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.BreweryStocks.Queries.GetBreweryStockByBeer
{
	public record GetBreweryStockByBeerQuery : IRequest<Result<GetBreweryStockByBeerDto>>
	{
		public string BeerName { get; set; }
		public string BreweryName { get; set; }
	}

	internal class GetBreweryStockByBeerQueryHandler : IRequestHandler<GetBreweryStockByBeerQuery, Result<GetBreweryStockByBeerDto>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBreweryStockRepository _breweryStockRepository;

		public GetBreweryStockByBeerQueryHandler(IUnitOfWork unitOfWork, IBreweryStockRepository breweryStockRepository, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_breweryStockRepository = breweryStockRepository;
		}

		public async Task<Result<GetBreweryStockByBeerDto>> Handle(GetBreweryStockByBeerQuery query, CancellationToken cancellationToken)
		{
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BeerName.ToLower());

			if (Beer == null)
				return await Result<GetBreweryStockByBeerDto>.FailureAsync("Beer Not Found.");

			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BreweryName.ToLower());

			if (Brewery == null)
				return await Result<GetBreweryStockByBeerDto>.FailureAsync("Brewery Not Found.");

			var BreweryStock = await _breweryStockRepository.GetBreweryStockByBeer(Brewery.Id, Beer.Id);

			if (BreweryStock == null)
				return await Result<GetBreweryStockByBeerDto>.FailureAsync("Brewery Stock Not Found.");

			var BreweryStockDto = _mapper.Map<GetBreweryStockByBeerDto>(BreweryStock);

			return await Result<GetBreweryStockByBeerDto>.SuccessAsync(BreweryStockDto, "Brewery Stock Found.");
		}
	}
}
