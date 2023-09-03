using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.WholesalerStocks.Queries.GetWholerStockByBeer
{
	public record GetWholesalerStockByBeerQuery : IRequest<Result<GetWholesalerStockByBeerDto>>
	{
        public string BeerName { get; set; }
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
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BeerName.ToLower());

			if (Beer == null)
				return await Result<GetWholesalerStockByBeerDto>.FailureAsync("Beer Not Found.");

			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<GetWholesalerStockByBeerDto>.FailureAsync("Wholesaler Not Found.");

			var WholesalerStock = await _wholesalerStockRepository.GetWholesalerStockByBeer(Wholesaler.Id, Beer.Id);

			if (WholesalerStock == null)
				return await Result<GetWholesalerStockByBeerDto>.FailureAsync("Wholesaler Stock Not Found.");

			var WholesalerStockDtos = _mapper.Map<GetWholesalerStockByBeerDto>(WholesalerStock);

			return await Result<GetWholesalerStockByBeerDto>.SuccessAsync(WholesalerStockDtos, "Wholesaler Stock Found.");
		}
	}
}
