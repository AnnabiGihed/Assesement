using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.WholesalerStocks.Command.UpdateWholesalerStock
{
	public record UpdateWholesalerStockCommand : IRequest<Result<int>>, IMapFrom<WholesalerStock>
	{
        public int Quantity { get; set; }
		public bool IsAddition { get; set; }
		public string BeerName { get; set; }
		public string WholesalerName { get; set; }
	}

	internal class UpdateWholesalerStockCommandHandler : IRequestHandler<UpdateWholesalerStockCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateWholesalerStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(UpdateWholesalerStockCommand command, CancellationToken cancellationToken)
		{
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			if (Beer == null)
				return await Result<int>.FailureAsync("Beer Not Found.");


			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<int>.FailureAsync("Wholesaler Not Found.");

			var WholesalerStock = await _unitOfWork.Repository<WholesalerStock>().Entities.FirstOrDefaultAsync(x => x.WholesalerId == Wholesaler.Id && x.BeerId == Beer.Id);

			if (WholesalerStock == null)
				return await Result<int>.FailureAsync("Wholesaler Stock doesn't exist, Request Creation.");

			if(command.IsAddition)
				WholesalerStock.Count += command.Quantity;
			else
				WholesalerStock.Count = command.Quantity;

			await _unitOfWork.Repository<WholesalerStock>().UpdateAsync(WholesalerStock);
			WholesalerStock.AddDomainEvent(new WholesalerStockUpdatedEvent(WholesalerStock));

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(WholesalerStock.Id, "Wholesaler Stock Updated.");
		}
	}
}
