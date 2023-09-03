using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.WholesalerStocks.Command.CreateWholesalerStock
{
	public record CreateWholesalerStockCommand : IRequest<Result<int>>, IMapFrom<WholesalerStock>
	{
        public int Quantity { get; set; }
        public string BeerName { get; set; }
        public string WholesalerName { get; set; }
    }

	internal class CreateWholesalerStockCommandHandler : IRequestHandler<CreateWholesalerStockCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateWholesalerStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(CreateWholesalerStockCommand command, CancellationToken cancellationToken)
		{
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			if (Beer == null)
				return await Result<int>.FailureAsync("Beer Not Found.");


			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<int>.FailureAsync("Wholesaler Not Found.");

			var WholesalerStock = await _unitOfWork.Repository<WholesalerStock>().Entities.FirstOrDefaultAsync(x => x.WholesalerId == Wholesaler.Id && x.BeerId == Beer.Id);

			if (WholesalerStock != null)
				return await Result<int>.FailureAsync("Wholesaler Stock Exist, Request an Update.");

			WholesalerStock = new WholesalerStock()
			{
				BeerId = Beer.Id,
				Count = command.Quantity,
				WholesalerId = Wholesaler.Id,
			};


			await _unitOfWork.Repository<WholesalerStock>().AddAsync(WholesalerStock);
			WholesalerStock.AddDomainEvent(new WholesalerStockCreatedEvent(WholesalerStock));

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(WholesalerStock.Id, "Wholesaler Stock Created.");
		}
	}
}
