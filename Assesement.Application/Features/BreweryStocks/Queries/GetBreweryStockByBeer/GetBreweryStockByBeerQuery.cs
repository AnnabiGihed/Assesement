using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Assessment.Application.Features.BreweryStocks.Queries.GetBreweryStockByBeer
{
	public record GetBreweryStockByBeerQuery : IRequest<Result<GetBreweryStockByBeerDto>>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Brewery Name is required")]
		[StringLength(255, ErrorMessage = "Brewery Name Must be between 2 and 255 characters.", MinimumLength = 2)]
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
			#region Check if beer Exist
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BeerName.ToLower());

			if (Beer == null)
				return await Result<GetBreweryStockByBeerDto>.FailureAsync("Beer Not Found.");
			#endregion

			#region Check if Brewery Exist
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BreweryName.ToLower());

			if (Brewery == null)
				return await Result<GetBreweryStockByBeerDto>.FailureAsync("Brewery Not Found.");
			#endregion

			#region Check If Brewery Stock of this beer Exist
			var BreweryStock = await _breweryStockRepository.GetBreweryStockByBeer(Brewery.Id, Beer.Id);

			if (BreweryStock == null)
				return await Result<GetBreweryStockByBeerDto>.FailureAsync("Brewery Stock Not Found.");
			#endregion

			var BreweryStockDto = _mapper.Map<GetBreweryStockByBeerDto>(BreweryStock);

			return await Result<GetBreweryStockByBeerDto>.SuccessAsync(BreweryStockDto, "Brewery Stock Found.");
		}
	}
}
