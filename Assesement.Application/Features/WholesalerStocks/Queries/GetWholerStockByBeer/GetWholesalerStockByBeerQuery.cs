using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.WholesalerStocks.Queries.GetWholerStockByBeer
{
	public record GetWholesalerStockByBeerQuery : IRequest<Result<GetWholesalerStockByBeerDto>>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Wholesaler Name is required")]
		[StringLength(255, ErrorMessage = "Wholesaler Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string WholesalerName { get; set; }
    }

	internal class GetWholesalerStockByBeerQueryHandler : IRequestHandler<GetWholesalerStockByBeerQuery, Result<GetWholesalerStockByBeerDto>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWholesalerStockRepository _wholesalerStockRepository;

		public GetWholesalerStockByBeerQueryHandler(IUnitOfWork unitOfWork, IWholesalerStockRepository wholesalerStockRepository, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_wholesalerStockRepository = wholesalerStockRepository;
		}

		public async Task<Result<GetWholesalerStockByBeerDto>> Handle(GetWholesalerStockByBeerQuery query, CancellationToken cancellationToken)
		{
			#region Check if beer exist
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BeerName.ToLower());

			if (Beer == null)
				return await Result<GetWholesalerStockByBeerDto>.FailureAsync("Beer Not Found.");
			#endregion

			#region Check if wholesaler exist
			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<GetWholesalerStockByBeerDto>.FailureAsync("Wholesaler Not Found.");
			#endregion

			#region Check if wholesaler have stock of this beer
			var WholesalerStock = await _wholesalerStockRepository.GetWholesalerStocksByBeer(Wholesaler.Id, Beer.Id);

			if (WholesalerStock == null)
				return await Result<GetWholesalerStockByBeerDto>.FailureAsync("Wholesaler Stock Not Found.");
			#endregion

			var WholesalerStockDtos = _mapper.Map<GetWholesalerStockByBeerDto>(WholesalerStock);

			return await Result<GetWholesalerStockByBeerDto>.SuccessAsync(WholesalerStockDtos, "Wholesaler Stock Found.");
		}
	}
}
