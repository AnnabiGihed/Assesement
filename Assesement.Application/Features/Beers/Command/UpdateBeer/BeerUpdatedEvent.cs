using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.Beers.Command.UpdateBeer
{
	public class BeerUpdatedEvent : BaseEvent
	{
		public Beer Beer { get; }

		public BeerUpdatedEvent(Beer beer)
		{
			Beer = beer;
		}
	}
}
