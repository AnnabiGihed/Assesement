using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.BreweryStocks.Command.CreateBreweryStock
{
	public class BreweryStockCreatedEvent : BaseEvent
	{
        public BreweryStock BreweryStock { get; set; }

        public BreweryStockCreatedEvent(BreweryStock breweryStock)
        {
            BreweryStock = breweryStock;
        }
    }
}
