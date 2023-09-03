using AutoMapper;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;

namespace Assessment.Application.Features.WholesalerStocks.Queries.GetWholesalerStockByBeerDetailed
{
	public class GetWholesalerStockByBeerDetailedDto : IMapFrom<WholesalerStock>
	{
		public int Count { get; set; }
		public Beer Beer { get; set; }
		public Wholesaler Wholesaler { get; set; }

		public void Mapping(Profile profile)
		{
			var c = profile.CreateMap<WholesalerStock, GetWholesalerStockByBeerDetailedDto>();
		}
	}
}
