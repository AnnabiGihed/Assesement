using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.Beers.Command.CreateBeer
{
	public class BeerCreatedEvent : BaseEvent
	{
        public Beer Beer { get; set; }

        public BeerCreatedEvent(Beer beer)
        {
            Beer= beer;
        }
    }
}
