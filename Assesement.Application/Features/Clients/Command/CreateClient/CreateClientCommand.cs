using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Clients.Command.CreateClient
{
	public record CreateClientCommand : IRequest<Result<int>>, IMapFrom<Client>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Client Name is required")]
		[StringLength(255, ErrorMessage = "Client Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string Name { get; set; }
	}
	internal class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<int>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(CreateClientCommand command, CancellationToken cancellationToken)
		{
			var Client = new Client()
			{
				Name = command.Name
			};

			await _unitOfWork.Repository<Client>().AddAsync(Client);
			Client.AddDomainEvent(new ClientCreatedEvent(Client));

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(Client.Id, "Client Created");
		}
	}
}
