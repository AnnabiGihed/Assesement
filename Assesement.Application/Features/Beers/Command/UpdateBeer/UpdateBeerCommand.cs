using MediatR;
using Assessment.Shared;
using Assessment.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Beers.Command.UpdateBeer
{
	public record UpdateBeerCommand : IRequest<Result<int>>
	{
		[Required(ErrorMessage = "Id is required")]
		public int Id { get; set; }

		[DataType(DataType.Text)]
		[StringLength(255, ErrorMessage = "Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string Name { get; set; }

		[DataType(DataType.Currency)]
		[Range(0.1, float.MaxValue, ErrorMessage = "Price must be superior to 0")]
		public float Price { get; set; }

		[Required(ErrorMessage = "Brewery Id is required")]
		public int BreweryStockId { get; set; }

		[Range(0, 100, ErrorMessage = "Alchool Percentage must be between 0 and 100%")]
		public float AlchoholPercentage { get; set; }
    }

	internal class UpdateBeerCommandHandler : IRequestHandler<UpdateBeerCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateBeerCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(UpdateBeerCommand command, CancellationToken cancellationToken)
		{
			//Check if beer exist
			var Beer = await _unitOfWork.Repository<Beer>().GetByIdAsync(command.Id);

			//Does Exist, Update it
			if (Beer != null)
			{
				Beer.Name = command.Name;
				Beer.Price = command.Price;
				Beer.BreweryStockId = command.BreweryStockId;
				Beer.AlchoholPercentage = command.AlchoholPercentage;
				
				await _unitOfWork.Repository<Beer>().UpdateAsync(Beer);
				Beer.AddDomainEvent(new BeerUpdatedEvent(Beer));

				await _unitOfWork.Save(cancellationToken);

				return await Result<int>.SuccessAsync(Beer.Id, "Beer Updated.");
			}
			else //Doesn't Exist, Update failed
			{
				return await Result<int>.FailureAsync("Beer Not Found.");
			}
		}
	}
}
