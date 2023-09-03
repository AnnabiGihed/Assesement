using AutoMapper;
using Assessment.Domain.Entities;
using Assessment.Application.Common.Mappings;

namespace Assessment.Application.Features.Clients.Queries.GetClientByName
{
	public class GetClientByNameDto : IMapFrom<Client>
	{
        public int Id { get; set; }
        public string Name { get; set; }

		public void Mapping(Profile profile)
		{
			var c = profile.CreateMap<Client, GetClientByNameDto>();
		}
	}
}
