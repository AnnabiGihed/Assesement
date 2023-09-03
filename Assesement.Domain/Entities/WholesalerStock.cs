using Assessment.Domain.Common;

namespace Assessment.Domain.Entities
{
	public class WholesalerStock : BaseAuditableEntity
	{
		public Beer Beer { get; set; }
		public int Count { get; set; }
		public int BeerId { get; set; }
		public int WholesalerId { get; set; }
		public Wholesaler Wholesaler { get; set; }
	}
}
