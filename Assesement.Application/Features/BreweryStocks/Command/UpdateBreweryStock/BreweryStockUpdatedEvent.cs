using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.BreweryStocks.Command.UpdateBreweryStock
{
	public class BreweryStockUpdatedEvent : BaseEvent
	{
		public BreweryStock BreweryStock { get; set; }

		public BreweryStockUpdatedEvent(BreweryStock breweryStock)
		{
			BreweryStock = breweryStock;
		}
	}
}
