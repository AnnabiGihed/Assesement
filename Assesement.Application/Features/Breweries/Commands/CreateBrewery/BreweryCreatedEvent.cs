using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.Breweries.Commands.CreateBrewery
{
    public class BreweryCreatedEvent : BaseEvent
    {
        public Brewery Brewery { get; }

        public BreweryCreatedEvent(Brewery brewery)
        {
            Brewery = brewery;
        }
    }
}
