using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Breweries.Commands.CreateBrewery
{
    public record CreateBreweryCommand : IRequest<Result<int>>, IMapFrom<Brewery>
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Brewery Name is required")]
		[StringLength(255, ErrorMessage = "Brewery Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string Name { get; set; }
    }

    internal class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBreweryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateBreweryCommand command, CancellationToken cancellationToken)
        {
            #region Check If Brewery With the provided Already exist
            var Brewery = _unitOfWork.Repository<Brewery>().Entities.FirstOrDefault(x => x.Name == command.Name);

            if(Brewery != null)
				return await Result<int>.FailureAsync("Brewery With the specified Name Already Exist.");
            #endregion

            #region Create Brewery
            Brewery = new Brewery()
            {
                Name = command.Name,
            };

            await _unitOfWork.Repository<Brewery>().AddAsync(Brewery);
			Brewery.AddDomainEvent(new BreweryCreatedEvent(Brewery));

            await _unitOfWork.Save(cancellationToken);
			#endregion

			return await Result<int>.SuccessAsync(Brewery.Id, "Brewery Created.");
        }
    }
}
