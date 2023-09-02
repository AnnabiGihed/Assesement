using Assesement.Domain.Common;

namespace Assesement.Domain.Entities
{
	public class Wholesaler : BaseAuditableEntity
	{
        public Wholesaler()
        {
            Sales = new List<WholesalerSale>();
			Purchase = new List<Transaction>();
			Stocks = new List<WholesalerStock>();
        }
        public string Name { get; set; }
		public IList<WholesalerSale> Sales { get; set; }
		public IList<Transaction> Purchase { get; set; }
		public IList<WholesalerStock> Stocks { get; set; }
	}
}
