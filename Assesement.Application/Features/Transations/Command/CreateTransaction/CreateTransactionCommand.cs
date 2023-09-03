using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Transations.Command.CreateTransaction
{
	public record CreateTransactionCommand : IRequest<Result<int>>, IMapFrom<Transaction>
	{
        public int Quantity { get; set; }
        public string BeerName { get; set; }
        public string BreweryName { get; set; }
        public string WholesalerName { get; set; }
    }

	internal class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
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

			if(brewerySpecificBeerStock.Count < command.Quantity)
				return await Result<int>.FailureAsync("Brewery doesn't have enough stock.");

			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name == command.WholesalerName);

			if (Wholesaler == null)
				return await Result<int>.FailureAsync("Wholesaler Not Found.");

			var Transaction = new Transaction()
			{
				BeerId = Beer.Id,
				BreweryId = Brewery.Id,
				Quantity = command.Quantity,
				WholesalerId = Wholesaler.Id
			};


			await _unitOfWork.Repository<Transaction>().AddAsync(Transaction);
			Transaction.AddDomainEvent(new TransactionCreateEvent(Transaction));

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(Transaction.Id, "Transaction Created.");
		}
	}
}
