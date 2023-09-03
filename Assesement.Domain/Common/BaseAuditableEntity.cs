using Assessment.Domain.Common.Interfaces;

namespace Assessment.Domain.Common
{
	public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
	{
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
