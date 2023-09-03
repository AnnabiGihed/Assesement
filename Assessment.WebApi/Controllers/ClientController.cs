using MediatR;
using Assessment.Shared;
using Microsoft.AspNetCore.Mvc;
using Assessment.Application.Features.Clients.Command.CreateClient;

namespace Assessment.WebApi.Controllers
{
	public class ClientController : ApiControllerBase
	{
		private readonly IMediator _mediator;

		public ClientController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		[ActionName("CreateClient")]
		public async Task<ActionResult<Result<int>>> CreateClient(CreateClientCommand command)
		{
			return await _mediator.Send(command);
		}
	}
}
