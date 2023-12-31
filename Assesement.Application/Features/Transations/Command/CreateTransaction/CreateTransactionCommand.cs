﻿using MediatR;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;
using Assessment.Application.Features.BreweryStocks.Command.UpdateBreweryStock;
using Assessment.Application.Features.WholesalerStocks.Command.CreateWholesalerStock;
using Assessment.Application.Features.WholesalerStocks.Command.UpdateWholesalerStock;

namespace Assessment.Application.Features.Transations.Command.CreateTransaction
{
	public record CreateTransactionCommand : IRequest<Result<int>>, IMapFrom<Transaction>
	{
		[Required(ErrorMessage = "Quantity is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
		public int Quantity { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Brewery Name is required")]
		[StringLength(255, ErrorMessage = "Brewery Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BreweryName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Wholesaler Name is required")]
		[StringLength(255, ErrorMessage = "Wholesaler Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string WholesalerName { get; set; }
    }

	internal class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
		{
			#region Check Brewery Existance
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BreweryName.ToLower());

			if (Brewery == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");
			#endregion

			#region Check Beer Existance
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			if (Beer == null)
				return await Result<int>.FailureAsync("Beer Not Found.");
			#endregion

			#region Check Brewery Stock
			var BreweryStock = await _unitOfWork.Repository<BreweryStock>().Entities.FirstOrDefaultAsync(x => x.BeerId == Beer.Id && x.BreweryId == Brewery.Id);

			if (BreweryStock == null)
				return await Result<int>.FailureAsync("Brewery doesn't have stock of this beer.");
			else if(BreweryStock.Count < command.Quantity)
				return await Result<int>.FailureAsync("Brewery doesn't have enough stock of this beer.");
			#endregion

			#region Check Wholesaler Existance
			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name == command.WholesalerName);

			if (Wholesaler == null)
				return await Result<int>.FailureAsync("Wholesaler Not Found.");
			#endregion

			#region Create Transaction
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
			#endregion

			#region Update Brewery Stock
			BreweryStock.Count -= command.Quantity;

			await _unitOfWork.Repository<BreweryStock>().UpdateAsync(BreweryStock);
			BreweryStock.AddDomainEvent(new BreweryStockUpdatedEvent(BreweryStock));

			await _unitOfWork.Save(cancellationToken);
			#endregion

			#region Update Wholesaler Stock
			//Check if Wholesaler have a stock of this beer
			var WholesalerStock = await _unitOfWork.Repository<WholesalerStock>().Entities.FirstOrDefaultAsync(x => x.BeerId == Beer.Id && x.WholesalerId == Wholesaler.Id);

			//No Stock Exists, Create it
			if (WholesalerStock == null)
			{
				WholesalerStock = new WholesalerStock()
				{
					BeerId = Beer.Id,
					WholesalerId = Wholesaler.Id,
					Count = command.Quantity
				};

				await _unitOfWork.Repository<WholesalerStock>().AddAsync(WholesalerStock);
				WholesalerStock.AddDomainEvent(new WholesalerStockCreatedEvent(WholesalerStock));

				await _unitOfWork.Save(cancellationToken);
			}
			else //Stock Exists, Update it
			{
				WholesalerStock.Count += command.Quantity;

				await _unitOfWork.Repository<WholesalerStock>().UpdateAsync(WholesalerStock);
				WholesalerStock.AddDomainEvent(new WholesalerStockUpdatedEvent(WholesalerStock));

				await _unitOfWork.Save(cancellationToken);
			}
			#endregion
			
			return await Result<int>.SuccessAsync(Transaction.Id, "Transaction Created.");
		}
	}
}
