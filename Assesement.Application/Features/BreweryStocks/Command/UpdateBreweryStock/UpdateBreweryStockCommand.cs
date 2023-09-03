using MediatR;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.BreweryStocks.Command.UpdateBreweryStock
{
	public record UpdateBreweryStockCommand : IRequest<Result<int>>, IMapFrom<BreweryStock>
	{
		[Required(ErrorMessage = "Stock is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0")]
		public int Count { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Brewery Name is required")]
		[StringLength(255, ErrorMessage = "Brewery Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BreweryName { get; set; }
	}

	internal class UpdateBreweryStockCommandHandler : IRequestHandler<UpdateBreweryStockCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateBreweryStockCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(UpdateBreweryStockCommand command, CancellationToken cancellationToken)
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

			#region Check Brewery Stock Existance
			var BreweryStock = await _unitOfWork.Repository<BreweryStock>().Entities.FirstOrDefaultAsync(x => x.BreweryId == Brewery.Id && x.BeerId == Beer.Id);

			if (BreweryStock == null)
				return await Result<int>.FailureAsync("Brewery Stock Not Found.");
			#endregion

			#region Update Brewery Stock
			BreweryStock.Count += command.Count;

			await _unitOfWork.Repository<BreweryStock>().UpdateAsync(BreweryStock);
			BreweryStock.AddDomainEvent(new BreweryStockUpdatedEvent(BreweryStock));

			await _unitOfWork.Save(cancellationToken);
			#endregion

			return await Result<int>.SuccessAsync(BreweryStock.Id, "Brewery Stock Updated.");
		}
	}
}
