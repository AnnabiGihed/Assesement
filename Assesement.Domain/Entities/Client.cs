using Assessment.Domain.Common;

namespace Assessment.Domain.Entities
{
	public class Client : BaseAuditableEntity
	{
        public Client()
        {
            Purchases = new List<WholesalerSale>();
        }
        public string Name { get; set; }
		public IList<WholesalerSale> Purchases { get; set; }
	}
}
