using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.WholesalerSales.Command.CreateWholesaleSale
{
	public class WholesaleSaleCreatedEvent : BaseEvent
	{
		public WholesalerSale WholesalerSale { get; set; }

		public WholesaleSaleCreatedEvent(WholesalerSale wholesalerSale)
		{
			WholesalerSale = wholesalerSale;
		}
	}
}
