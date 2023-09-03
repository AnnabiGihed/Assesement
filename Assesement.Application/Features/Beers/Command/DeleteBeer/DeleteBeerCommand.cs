using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;


namespace Assessment.Application.Features.Beers.Command.DeleteBeer
{
	public record DeleteBeerCommand : IRequest<Result<int>>, IMapFrom<Beer>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }
	}

	internal class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DeleteBeerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(DeleteBeerCommand command, CancellationToken cancellationToken)
		{
			//Check if Beer exist
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.BeerName.ToLower());

			//Beer Exist, Delete it
			if (Beer != null)
			{
				await _unitOfWork.Repository<Beer>().DeleteAsync(Beer);
				Beer.AddDomainEvent(new PlayerDeletedEvent(Beer));

				await _unitOfWork.Save(cancellationToken);

				return await Result<int>.SuccessAsync(Beer.Id, "Beer Deleted");
			}
			else //Beer Doesn't Exist, Delete Failed
			{
				return await Result<int>.FailureAsync("Beer Not Found.");
			}
		}
	}
}
