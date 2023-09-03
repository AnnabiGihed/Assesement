using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.Clients.Command.CreateClient
{
	public class ClientCreatedEvent : BaseEvent
	{
		public Client Client { get; set; }

		public ClientCreatedEvent(Client client)
		{
			Client = client;
		}
	}
}
