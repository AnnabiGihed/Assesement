﻿using Assesement.Domain.Common;

namespace Assesement.Domain.Entities
{
	public class BreweryStock : BaseAuditableEntity
	{
		public Beer Beer { get; set; }
		public int Count { get; set; }
		public int BeerId { get; set; }
		public int BreweryId { get; set; }
		public Brewery Brewery { get; set; }
	}
}
