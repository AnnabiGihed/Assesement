using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.WholesalerStocks.Command.UpdateWholesalerStock
{
	public class WholesalerStockUpdatedEvent : BaseEvent
	{
        public WholesalerStock WholesalerStock { get; set; }

        public WholesalerStockUpdatedEvent(WholesalerStock wholesalerStock)
        {
			WholesalerStock = wholesalerStock;
		}
    }
}
