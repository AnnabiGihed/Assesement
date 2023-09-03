using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Assessment.Application.Interfaces.Repositories;

namespace Assessment.Application.Features.Breweries.Queries.GetAllBreweries
{
    public record GetAllBreweriesQuery : IRequest<Result<List<GetAllBreweriesDto>>>;
    internal class GetAllBreweriesQueryHandler : IRequestHandler<GetAllBreweriesQuery, Result<List<GetAllBreweriesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllBreweriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllBreweriesDto>>> Handle(GetAllBreweriesQuery query, CancellationToken cancellationToken)
        {
            var Breweries = await _unitOfWork.Repository<Brewery>().Entities
                   .ProjectTo<GetAllBreweriesDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllBreweriesDto>>.SuccessAsync(Breweries);
        }
    }
}
