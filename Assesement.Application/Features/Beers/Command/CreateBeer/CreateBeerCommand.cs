using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

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
		private readonly IMapper _mapper;

		public CreateBeerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(CreateBeerCommand command, CancellationToken cancellationToken)
		{
			var Brewery = await _unitOfWork.Repository<Brewery>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BreweryName.ToLower());

			if(Brewery == null)
				return await Result<int>.FailureAsync("Brewery Not Found.");
			var beer = new Beer()
			{
				Name = command.Name,
				Price = command.Price,
				AlchoholPercentage = command.AlchoolPercentage,
			};
			
			await _unitOfWork.Repository<Beer>().AddAsync(beer);
			beer.AddDomainEvent(new BeerCreatedEvent(beer));
			
			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(beer.Id, "Beer Created");
		}
	}
}
