using MediatR;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.WholesalerStocks.Command.CreateWholesalerStock
{
	public record CreateWholesalerStockCommand : IRequest<Result<int>>, IMapFrom<WholesalerStock>
	{
		[Required(ErrorMessage = "Quantity is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
		public int Quantity { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Wholesaler Name is required")]
		[StringLength(255, ErrorMessage = "Wholesaler Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string WholesalerName { get; set; }
    }

	internal class CreateWholesalerStockCommandHandler : IRequestHandler<CreateWholesalerStockCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateWholesalerStockCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(CreateWholesalerStockCommand command, CancellationToken cancellationToken)
		{
			#region Check if beer exist
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			if (Beer == null)
				return await Result<int>.FailureAsync("Beer Not Found.");
			#endregion

			#region Check if Wholesaler exist
			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<int>.FailureAsync("Wholesaler Not Found.");
			#endregion

			#region Check if Wholesaler have stock of this beer
			var WholesalerStock = await _unitOfWork.Repository<WholesalerStock>().Entities.FirstOrDefaultAsync(x => x.WholesalerId == Wholesaler.Id && x.BeerId == Beer.Id);

			if (WholesalerStock != null)
				return await Result<int>.FailureAsync("Wholesaler Stock Exist, Request an Update.");
			#endregion

			#region Create Wholesaler Stock of this beer
			WholesalerStock = new WholesalerStock()
			{
				BeerId = Beer.Id,
				Count = command.Quantity,
				WholesalerId = Wholesaler.Id,
			};

			await _unitOfWork.Repository<WholesalerStock>().AddAsync(WholesalerStock);
			WholesalerStock.AddDomainEvent(new WholesalerStockCreatedEvent(WholesalerStock));

			await _unitOfWork.Save(cancellationToken);
			#endregion

			return await Result<int>.SuccessAsync(WholesalerStock.Id, "Wholesaler Stock Created.");
		}
	}
}
