using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.BreweryStocks.Command.UpdateBreweryStock
{
	public record UpdateBreweryStockCommand : IRequest<Result<int>>, IMapFrom<BreweryStock>
	{
		public int Count { get; set; }
		public string BeerName { get; set; }
		public string BreweryName { get; set; }
	}

	internal class UpdateBreweryStockCommandHandler : IRequestHandler<UpdateBreweryStockCommand, Result<int>>
	{
		private readonly IBreweryStockRepository _breweryStockRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateBreweryStockCommandHandler(IUnitOfWork unitOfWork, IBreweryStockRepository breweryStockRepository,IMapper mapper)
		{
			_breweryStockRepository = breweryStockRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(UpdateBreweryStockCommand command, CancellationToken cancellationToken)
		{
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BreweryName.ToLower());

			if (Brewery == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");

			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			if (Beer == null)
				return await Result<int>.FailureAsync("Beer Not Found.");

			var brewerySpecificBeerStock = await _unitOfWork.Repository<BreweryStock>().Entities.FirstOrDefaultAsync(x => x.BreweryId == Brewery.Id && x.BeerId == Beer.Id);

			if (brewerySpecificBeerStock == null)
				return await Result<int>.FailureAsync("Brewery Stock Not Found.");

			brewerySpecificBeerStock.Count += command.Count;

			await _unitOfWork.Repository<BreweryStock>().UpdateAsync(brewerySpecificBeerStock);
			brewerySpecificBeerStock.AddDomainEvent(new BreweryStockUpdatedEvent(brewerySpecificBeerStock));

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(brewerySpecificBeerStock.Id, "Brewery Stock Updated.");
		}
	}
}
