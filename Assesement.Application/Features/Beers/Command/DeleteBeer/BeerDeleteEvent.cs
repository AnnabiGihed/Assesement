using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.Beers.Command.DeleteBeer
{
	public class PlayerDeletedEvent : BaseEvent
	{
		public Beer Beer { get; }

		public PlayerDeletedEvent(Beer beer)
		{
			Beer = beer;
		}
	}
}
