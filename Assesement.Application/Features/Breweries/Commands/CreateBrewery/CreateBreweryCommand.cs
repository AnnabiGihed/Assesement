using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;
using Assessment.Domain.Entities;
using Assessment.Shared;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Application.Features.Breweries.Commands.CreateBrewery
{
    public record CreateBreweryCommand : IRequest<Result<int>>, IMapFrom<Brewery>
    {
        public string Name { get; set; }
    }

    internal class CreateBreweryCommandHandler : IRequestHandler<CreateBreweryCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBreweryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateBreweryCommand command, CancellationToken cancellationToken)
        {
            var brewery = new Brewery()
            {
                Name = command.Name,
            };

            await _unitOfWork.Repository<Brewery>().AddAsync(brewery);
            brewery.AddDomainEvent(new BreweryCreatedEvent(brewery));

            await _unitOfWork.Save(cancellationToken);

            return await Result<int>.SuccessAsync(brewery.Id, "Brewery Created.");
        }
    }
}
