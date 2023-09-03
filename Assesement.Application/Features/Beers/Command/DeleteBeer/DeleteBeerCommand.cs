using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Beers.Command.DeleteBeer
{
	public record DeleteBeerCommand : IRequest<Result<int>>, IMapFrom<Beer>
	{
		public int Id { get; set; }
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
			var beer = await _unitOfWork.Repository<Beer>().GetByIdAsync(command.Id);
			if (beer != null)
			{
				await _unitOfWork.Repository<Beer>().DeleteAsync(beer);
				beer.AddDomainEvent(new PlayerDeletedEvent(beer));

				await _unitOfWork.Save(cancellationToken);

				return await Result<int>.SuccessAsync(beer.Id, "Beer Deleted");
			}
			else
			{
				return await Result<int>.FailureAsync("Beer Not Found.");
			}
		}
	}
}
