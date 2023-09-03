using MediatR;
using Assessment.Shared;
using Microsoft.AspNetCore.Mvc;
using Assessment.Application.Features.Beers.Command.DeleteBeer;
using Assessment.Application.Features.Beers.Command.CreateBeer;
using Assessment.Application.Features.Breweries.Queries.GetAllBreweries;
using Assessment.Application.Features.Transations.Command.CreateTransaction;
using Assessment.Application.Features.BreweryStocks.Command.UpdateBreweryStock;
using Assessment.Application.Features.BreweryStocks.Queries.GetBreweryStockByBeer;
using Assessment.Application.Features.Breweries.Queries.GetAllBreweriesAndTheirBeers;

namespace Assessment.WebApi.Controllers
{
    public class BreweryController : ApiControllerBase
	{
		private readonly IMediator _mediator;

		public BreweryController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpGet]
		[ActionName("GetBreweries")]
		public async Task<ActionResult<Result<List<GetAllBreweriesDto>>>> GetBreweries()
		{
			return await _mediator.Send(new GetAllBreweriesQuery());
		}

		[HttpGet]
		[ActionName("GetBreweriesAndTheirBeers")]
		public async Task<ActionResult<Result<List<GetAllBreweriesAndTheirBeersDto>>>> GetBreweryAndTheirBeers()
		{
			return await _mediator.Send(new GetAllBreweriesAndTheirBeersQuery());
		}

		[HttpPost]
		[ActionName("CreateBeer")]
		public async Task<ActionResult<Result<int>>> CreateBeer(CreateBeerCommand command)
		{
			return await _mediator.Send(command);
		}

		[HttpDelete]
		[ActionName("DeleteBeer")]
		public async Task<ActionResult<Result<int>>> DeleteBeer(DeleteBeerCommand command)
		{
			return await _mediator.Send(command);
		}

		[HttpPut]
		[ActionName("UpdateBeerStock")]
		public async Task<ActionResult<Result<int>>> UpdateBeerStock(UpdateBreweryStockCommand command)
		{
			return await _mediator.Send(command);
		}

		[HttpGet]
		[ActionName("GetBreweryStockByBeer")]
		public async Task<ActionResult<Result<GetBreweryStockByBeerDto>>> GetBreweryStockByBeer(GetBreweryStockByBeerQuery query)
		{
			return await _mediator.Send(query);
		}

		[HttpPost]
		[ActionName("SellBearToWholeSaler")]
		public async Task<ActionResult<Result<int>>> SellBearToWholeSaler(CreateTransactionCommand command)
		{
			return await _mediator.Send(command);
		}
	}
}
