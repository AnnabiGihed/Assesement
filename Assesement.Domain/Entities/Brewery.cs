using Assessment.Domain.Common;

namespace Assessment.Domain.Entities
{
	public class Brewery : BaseAuditableEntity
	{
        public Brewery()
        {
            Sales = new List<Transaction>();
			Stocks = new List<BreweryStock>();
        }
        public string Name { get; set; }
		public IList<Transaction> Sales { get; set; }
		public IList<BreweryStock> Stocks { get; set; }
	}
}
