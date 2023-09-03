using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.WholesalerStocks.Command.CreateWholesalerStock
{
	public class WholesalerStockCreatedEvent : BaseEvent
	{
        public WholesalerStock WholesalerStock { get; set; }

        public WholesalerStockCreatedEvent(WholesalerStock wholesalerStock)
        {
			WholesalerStock = wholesalerStock;
		}
    }
}
