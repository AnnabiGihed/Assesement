using MediatR;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;
using Assessment.Application.Features.Beers.Command.UpdateBeer;
using Assessment.Application.Features.BreweryStocks.Command.CreateBreweryStock;

namespace Assessment.Application.Features.Beers.Command.CreateBeer
{
	public record CreateBeerCommand : IRequest<Result<int>>, IMapFrom<Beer>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string Name { get; set; }

		[DataType(DataType.Currency)]
		[Required(ErrorMessage = "Price is required")]
		[Range(0.1, float.MaxValue, ErrorMessage = "Price must be superior to 0")]
		public float Price { get; set; }

		[Required(ErrorMessage = "Stock is required")]
		[Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than 0")]
		public int StockCount { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Brewery Name is required")]
		[StringLength(255, ErrorMessage = "Brewery Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BreweryName { get; set; }

		[Required(ErrorMessage = "Alchohol Percentage is required")]
		[Range(0, 100, ErrorMessage = "Alchool Percentage must be between 0 and 100%")]
		public float AlchoolPercentage { get; set; }
	}
	internal class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateBeerCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(CreateBeerCommand command, CancellationToken cancellationToken)
		{
			#region Check Brewery exist
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BreweryName.ToLower());

			if(Brewery == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");
			#endregion

			#region Create Beer
			var Beer = new Beer()
			{
				Name = command.Name,
				Price = command.Price,
				AlchoholPercentage = command.AlchoolPercentage,
			};
			
			await _unitOfWork.Repository<Beer>().AddAsync(Beer);
			Beer.AddDomainEvent(new BeerCreatedEvent(Beer));
			
			await _unitOfWork.Save(cancellationToken);
			#endregion

			#region Check if Brewery Stock Already exists, And Create or Update
			var BreweryStock = await _unitOfWork.Repository<BreweryStock>().Entities.FirstOrDefaultAsync(x => x.BreweryId == Brewery.Id && x.Beer.Name.ToLower() == Beer.Name.ToLower());

			if (BreweryStock == null)
			{
				BreweryStock = new BreweryStock()
				{
					Count = command.StockCount,
					BreweryId = Brewery.Id,
					BeerId = Beer.Id
				};

				await _unitOfWork.Repository<BreweryStock>().AddAsync(BreweryStock);
				BreweryStock.AddDomainEvent(new BreweryStockCreatedEvent(BreweryStock));

				await _unitOfWork.Save(cancellationToken);
			}
			else
			{
				BreweryStock.Count = command.StockCount;

				await _unitOfWork.Repository<BreweryStock>().AddAsync(BreweryStock);
				BreweryStock.AddDomainEvent(new BreweryStockCreatedEvent(BreweryStock));

				await _unitOfWork.Save(cancellationToken);
			}
			#endregion

			#region Update Beer BreweryStock Id
			Beer.BreweryStockId = BreweryStock.Id;

			await _unitOfWork.Repository<Beer>().UpdateAsync(Beer);
			Beer.AddDomainEvent(new BeerUpdatedEvent(Beer));

			await _unitOfWork.Save(cancellationToken);
			#endregion

			return await Result<int>.SuccessAsync(Beer.Id, "Beer Created, And Brewery Stock Updated");
		}
	}
}
