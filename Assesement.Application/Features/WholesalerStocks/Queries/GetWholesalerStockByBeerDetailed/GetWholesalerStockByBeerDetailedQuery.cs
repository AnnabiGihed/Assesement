﻿using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.WholesalerStocks.Queries.GetWholesalerStockByBeerDetailed
{
	public record GetWholesalerStockByBeerDetailedQuery : IRequest<Result<GetWholesalerStockByBeerDetailedDto>>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Beer Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string BeerName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Beer Name is required")]
		[StringLength(255, ErrorMessage = "Wholesaler Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string WholesalerName { get; set; }
	}

	internal class GetWholesalerStockByBeerDetailedQueryHandler : IRequestHandler<GetWholesalerStockByBeerDetailedQuery, Result<GetWholesalerStockByBeerDetailedDto>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWholesalerStockRepository _wholesalerStockRepository;

		public GetWholesalerStockByBeerDetailedQueryHandler(IUnitOfWork unitOfWork, IWholesalerStockRepository wholesalerStockRepository, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_wholesalerStockRepository = wholesalerStockRepository;
		}

		public async Task<Result<GetWholesalerStockByBeerDetailedDto>> Handle(GetWholesalerStockByBeerDetailedQuery query, CancellationToken cancellationToken)
		{
			#region Check if Beer exist
			var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.BeerName.ToLower());

			if (Beer == null)
				return await Result<GetWholesalerStockByBeerDetailedDto>.FailureAsync("Beer Not Found.");
			#endregion

			#region Check if wholesaler Exist
			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == query.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<GetWholesalerStockByBeerDetailedDto>.FailureAsync("Wholesaler Not Found.");
			#endregion

			#region Check if wholesaler Has stock of this beer
			var WholesalerStock = await _wholesalerStockRepository.GetWholesalerStocksByBeer(Wholesaler.Id, Beer.Id);

			if (WholesalerStock == null)
				return await Result<GetWholesalerStockByBeerDetailedDto>.FailureAsync("Wholesaler Stock Not Found.");
			#endregion

			var WholesalerStockDtos = _mapper.Map<GetWholesalerStockByBeerDetailedDto>(WholesalerStock);

			return await Result<GetWholesalerStockByBeerDetailedDto>.SuccessAsync(WholesalerStockDtos, "Wholesaler Stock Found.");
		}
	}
}
