using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;

namespace Assessment.Application.Features.Breweries.Queries.GetAllBreweries
{
    public class GetAllBreweriesDto : IMapFrom<Brewery>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
