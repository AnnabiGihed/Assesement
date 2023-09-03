using MediatR;
using AutoMapper;
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
		private readonly IMapper _mapper;

		public UpdateBeerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(UpdateBeerCommand command, CancellationToken cancellationToken)
		{
			var beer = await _unitOfWork.Repository<Beer>().GetByIdAsync(command.Id);
			if (beer != null)
			{
				beer.Name = command.Name;
				beer.Price = command.Price;
				beer.BreweryStockId = command.BreweryStockId;
				beer.AlchoholPercentage = command.AlchoholPercentage;
				

				await _unitOfWork.Repository<Beer>().UpdateAsync(beer);
				beer.AddDomainEvent(new BeerUpdatedEvent(beer));

				await _unitOfWork.Save(cancellationToken);

				return await Result<int>.SuccessAsync(beer.Id, "Beer Updated.");
			}
			else
			{
				return await Result<int>.FailureAsync("Beer Not Found.");
			}
		}
	}
}
