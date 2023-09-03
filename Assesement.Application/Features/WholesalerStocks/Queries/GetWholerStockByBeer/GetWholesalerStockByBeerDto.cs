using AutoMapper;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;

namespace Assessment.Application.Features.WholesalerStocks.Queries.GetWholerStockByBeer
{
	public class GetWholesalerStockByBeerDto : IMapFrom<WholesalerStock>
	{
        public int Count { get; set; }
		public string BeerName { get; set; }
        public string Wholesaler { get; set; }
		public int WholesalerStockId { get; set; }

		public void Mapping(Profile profile)
		{
			var c = profile.CreateMap<WholesalerStock, GetWholesalerStockByBeerDto>()
				.ForMember(d => d.BeerName, opt => opt.MapFrom(src => src.Beer.Name))
				.ForMember(d => d.Wholesaler, opt => opt.MapFrom(src => src.Wholesaler.Name));
		}
	}
}
