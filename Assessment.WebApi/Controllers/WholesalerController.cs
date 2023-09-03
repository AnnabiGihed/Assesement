using MediatR;
using Assessment.Shared;
using Microsoft.AspNetCore.Mvc;
using Assessment.Application.Features.WholesalerSales.Command.CreateWholesaleSale;
using Assessment.Application.Features.WholesalerStocks.Queries.GetWholerStockByBeer;
using Assessment.Application.Features.WholesalerStocks.Command.UpdateWholesalerStock;

namespace Assessment.WebApi.Controllers
{
	public class WholesalerController : ApiControllerBase
	{
		private readonly IMediator _mediator;

		public WholesalerController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[ActionName("GetWholesalerStockByBeer")]
		public async Task<ActionResult<Result<GetWholesalerStockByBeerDto>>> GetWholesalerStockByBeer(GetWholesalerStockByBeerQuery query)
		{
			return await _mediator.Send(query);
		}

		[HttpPut]
		[ActionName("UpdateWholesalerBeerStock")]
		public async Task<ActionResult<Result<int>>> UpdateWholesalerBeerStock(UpdateWholesalerStockCommand command)
		{
			return await _mediator.Send(command);
		}

		[HttpPost]
		[ActionName("SellBeerToClient")]
		public async Task<ActionResult<Result<CreateWholesalerSaleDto>>> SellBeerToClient(CreateWholesalerSaleCommand command)
		{
			return await _mediator.Send(command);
		}
	}
}
