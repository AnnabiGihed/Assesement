using Assessment.Domain.Common;

namespace Assessment.Domain.Entities
{
	public class Beer : BaseAuditableEntity
	{
        public Beer()
        {
			Stocks = new List<WholesalerStock>();
			WholesalerSales = new List<WholesalerSale>();
        }
        public string Name { get; set; }
		public float Price { get; set; }
		public int BreweryStockId { get; set; }
		public float AlchoholPercentage { get; set; }
		public BreweryStock BreweryStock { get; set; }
		public IList<WholesalerStock> Stocks { get; set; }
		public IList<WholesalerSale> WholesalerSales { get; set; }
	}
}
