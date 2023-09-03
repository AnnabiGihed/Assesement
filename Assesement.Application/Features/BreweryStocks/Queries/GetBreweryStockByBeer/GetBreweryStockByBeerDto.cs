using AutoMapper;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;

namespace Assessment.Application.Features.BreweryStocks.Queries.GetBreweryStockByBeer
{
	public class GetBreweryStockByBeerDto : IMapFrom<BreweryStock>
	{
		public int Count { get; set; }
		public string BeerName { get; set; }
		public string BreweryName { get; set; }
		public int BreweryStockId { get; set; }

		public void Mapping(Profile profile)
		{
			var c = profile.CreateMap<BreweryStock, GetBreweryStockByBeerDto>()
				.ForMember(d => d.BeerName, opt => opt.MapFrom(src => src.Beer.Name))
				.ForMember(d => d.BreweryName, opt => opt.MapFrom(src => src.Brewery.Name));
		}
	}
}
