using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Application.Features.BreweryStocks.Command.CreateBreweryStock
{
	public record CreateBreweryStockCommand : IRequest<Result<int>>, IMapFrom<BreweryStock>
	{
        public int Count { get; set; }
        public int BeerId { get; set; }
        public string BreweryName { get; set; }
    }

	internal class CreateBreweryStockCommandHandler : IRequestHandler<CreateBreweryStockCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateBreweryStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(CreateBreweryStockCommand command, CancellationToken cancellationToken)
		{
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BreweryName.ToLower());

			if (Brewery == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");

			var breweryStock = new BreweryStock()
			{
				Count = command.Count,
				BreweryId = Brewery.Id,
				BeerId = command.BeerId,	
			};
		

			await _unitOfWork.Repository<BreweryStock>().AddAsync(breweryStock);
			breweryStock.AddDomainEvent(new BreweryStockCreatedEvent(breweryStock));

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(breweryStock.Id, "Brewery Stock Created.");
		}
	}
}
