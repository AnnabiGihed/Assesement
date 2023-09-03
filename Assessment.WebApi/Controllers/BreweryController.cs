using MediatR;
using Assessment.Shared;
using Microsoft.AspNetCore.Mvc;
using Assessment.Application.Features.Breweries.Queries;
using Assessment.Application.Features.Breweries.Queries.GetAllBreweries;
using Assessment.Application.Features.Breweries.Queries.GetAllBreweriesAndTheirBeers;
using Assessment.Application.Features.Beers.Command.CreateBeer;
using Assessment.Application.Features.BreweryStocks.Command.CreateBreweryStock;
using Assessment.Application.Features.Beers.Command.UpdateBeer;
using Assessment.Application.Features.Beers.Command.DeleteBeer;

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
			//Create Beer First
			var BeerCreationRes =  await _mediator.Send(command);

			if (!BeerCreationRes.Succeeded)
				return BeerCreationRes;

			//create CreateBreweryStockCommand
			CreateBreweryStockCommand CrBreweryStockCmd = new CreateBreweryStockCommand()
			{
				Count = command.StockCount,
				BreweryName = command.BreweryName,
				BeerId = BeerCreationRes.Data,
			};

			//Create Brewery Stock
			var BreweryStockCreationRes = await _mediator.Send(CrBreweryStockCmd);

			if (!BreweryStockCreationRes.Succeeded)
			{
				//Call Deletion of beer
				return BreweryStockCreationRes;
			}

			//Create UpdateBeerCommand
			UpdateBeerCommand UpBeerCmd = new UpdateBeerCommand()
			{
				Name = command.Name,
				Price = command.Price,
				Id = BeerCreationRes.Data,
				BreweryStockId = BreweryStockCreationRes.Data,
				AlchoholPercentage = command.AlchoolPercentage
			};

			var BeerUpdateRes = await _mediator.Send(UpBeerCmd);

			if(!BeerUpdateRes.Succeeded)
			{
				//Call Deletion of beer & BreweryStock
				return BeerUpdateRes;
			}


			return BeerCreationRes;
		}

		[HttpDelete]
		[ActionName("DeleteBeer")]
		public async Task<ActionResult<Result<int>>> DeleteBeer(DeleteBeerCommand command)
		{
			return await _mediator.Send(command);
		}
	}
}
