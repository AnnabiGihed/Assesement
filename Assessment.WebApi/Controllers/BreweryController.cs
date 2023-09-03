using MediatR;
using Assessment.Shared;
using Microsoft.AspNetCore.Mvc;
using Assessment.Application.Features.Beers.Command.UpdateBeer;
using Assessment.Application.Features.Beers.Command.DeleteBeer;
using Assessment.Application.Features.Beers.Command.CreateBeer;
using Assessment.Application.Features.Breweries.Queries.GetAllBreweries;
using Assessment.Application.Features.Transations.Command.CreateTransaction;
using Assessment.Application.Features.BreweryStocks.Command.CreateBreweryStock;
using Assessment.Application.Features.BreweryStocks.Command.UpdateBreweryStock;
using Assessment.Application.Features.BreweryStocks.Queries.GetBreweryStockByBeer;
using Assessment.Application.Features.WholesalerStocks.Queries.GetWholerStockByBeer;
using Assessment.Application.Features.Breweries.Queries.GetAllBreweriesAndTheirBeers;
using Assessment.Application.Features.WholesalerStocks.Command.UpdateWholesalerStock;

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
			//Retrive Brewery Stock
			var BreweryStockQuery = new GetBreweryStockByBeerQuery() { BeerName = command.BeerName, BreweryName = command.BreweryName };
			var GetBSQueryRes = await _mediator.Send(BreweryStockQuery);

			if (GetBSQueryRes.Succeeded && GetBSQueryRes.Data == null)
				return await Result<int>.FailureAsync("Brewery doesn't have stock of this beer");
			else if(GetBSQueryRes.Succeeded && GetBSQueryRes.Data != null && GetBSQueryRes.Data.Count < command.Quantity)
				return await Result<int>.FailureAsync("Brewery doesn't have enough stock of this beer");

			//Create The Transaction
			var TransCreationRes =  await _mediator.Send(command);

			if(!TransCreationRes.Succeeded)
				return TransCreationRes;

			//Try To Update
			var UpBrStockComd = new UpdateBreweryStockCommand()
			{
				Count = command.Quantity * -1,
				BeerName = command.BeerName,
				BreweryName = command.BreweryName,
			};

			var BreweryStockUpdateRes = await _mediator.Send(UpBrStockComd);

			if (!BreweryStockUpdateRes.Succeeded)
				return BreweryStockUpdateRes;

			var WholeSalerStockQuery = new GetWholesalerStockByBeerQuery() { BeerName = command.BeerName, WholesalerName = command.WholesalerName };

			var GetWSSRes = await _mediator.Send(WholeSalerStockQuery);

			if(GetWSSRes.Succeeded && GetWSSRes.Data != null)
			{
				var UpWSSComd = new UpdateWholesalerStockCommand()
				{
					BeerName = command.BeerName,
					WholesalerName = command.WholesalerName,
					Quantity = command.Quantity
				};

				var UpWSSRes = await _mediator.Send(UpWSSComd);

				return UpWSSRes;
			}

			return BreweryStockUpdateRes;
		}
	}
}
