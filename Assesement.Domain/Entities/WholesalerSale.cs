using Assessment.Domain.Common;

namespace Assessment.Domain.Entities
{
	public class WholesalerSale : BaseAuditableEntity
	{
		public int BeerId { get; set; }
		public int Quantity { get; set; }
		public Beer SoldBeer { get; set; }
		public int ClientId { get; set; }
		public Client Client { get; set; }
		public float UnitePrice { get; set; }
		public int WholesalerId { get; set; }
		public Wholesaler Wholesaler { get; set; }
	}
}
