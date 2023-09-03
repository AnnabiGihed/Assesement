using AutoMapper;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;

namespace Assessment.Application.Features.WholesalerSales.Command.CreateWholesaleSale
{
	public class CreateWholesalerSaleDto : IMapFrom<WholesalerSale>
	{
		public string Client { get; set; }
		public float TotalPrice { get; set; }
		public string WholeSaler { get; set; }
        public Dictionary<string,int> OrderContent { get; set; }

        public void Mapping(Profile profile)
		{
			var c = profile.CreateMap<WholesalerSale, CreateWholesalerSaleDto>();
		}
	}
}
