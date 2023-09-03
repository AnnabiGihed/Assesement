using MediatR;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.BreweryStocks.Command.CreateBreweryStock
{
	public record CreateBreweryStockCommand : IRequest<Result<int>>, IMapFrom<BreweryStock>
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
    }

	internal class CreateBreweryStockCommandHandler : IRequestHandler<CreateBreweryStockCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateBreweryStockCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(CreateBreweryStockCommand command, CancellationToken cancellationToken)
		{
			#region Check Brewery Existance
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BreweryName.ToLower());

			if (Brewery == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");
			#endregion

			#region Check Beer Existance
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			if (Beer == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");
			#endregion

			#region Create BreweryStock
			var breweryStock = new BreweryStock()
			{
				Count = command.Quantity,
				BreweryId = Brewery.Id,
				BeerId = Beer.Id
			};
		

			await _unitOfWork.Repository<BreweryStock>().AddAsync(breweryStock);
			breweryStock.AddDomainEvent(new BreweryStockCreatedEvent(breweryStock));

			await _unitOfWork.Save(cancellationToken);
			#endregion

			return await Result<int>.SuccessAsync(breweryStock.Id, "Brewery Stock Created.");
		}
	}
}
