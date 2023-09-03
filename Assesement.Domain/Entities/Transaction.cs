using Assessment.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Domain.Entities
{
	public class Transaction : BaseAuditableEntity
	{
		public Beer Beer { get; set; }
		public int BeerId { get; set; }
		public int Quantity { get; set; }
		public int BreweryId { get; set; }
		public Brewery Brewery { get; set; }
		public int WholesalerId { get; set; }
		public Wholesaler WholeSaler { get; set; }
	}
}
