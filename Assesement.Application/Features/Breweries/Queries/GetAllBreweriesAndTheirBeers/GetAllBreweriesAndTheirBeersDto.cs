using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;
using AutoMapper;

namespace Assessment.Application.Features.Breweries.Queries.GetAllBreweriesAndTheirBeers
{
    public class GetAllBreweriesBreweryStockDto : IMapFrom<BreweryStock>
    {
        public string BeerName { get; set; }
		public int Count { get; set; }

		public void Mapping(Profile profile)
		{
			var c = profile.CreateMap<BreweryStock, GetAllBreweriesBreweryStockDto>()
				.ForMember(d => d.BeerName, opt => opt.MapFrom(src => src.Beer.Name));
		}
	}
	public class GetAllBreweriesAndTheirBeersDto : IMapFrom<Brewery>
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<GetAllBreweriesBreweryStockDto> Stock { get; set; }

        public void Mapping(Profile profile)
        {
			var c = profile.CreateMap<Brewery, GetAllBreweriesAndTheirBeersDto>()
				.ForMember(d => d.Stock, opt => opt.MapFrom(src => src.Stocks));
		}
    }
}
