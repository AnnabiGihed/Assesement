using Moq;
using AutoMapper;
using NUnit.Framework;
using FluentAssertions;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;
using Assessment.Application.Features.Breweries.Commands.CreateBrewery;

namespace Assessment.UnitTesting
{

	[TestClass]
	public class UnitTest1
	{
		private readonly IMapper _mapper;
		private readonly List<Brewery> breweries;
		private IQueryable<Brewery> breweriesQuery;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;
		private readonly Mock<IGenericRepository<Brewery>> _breweryRepositoryMock;

		public UnitTest1()
		{
			breweries = new List<Brewery>()
			{
				new Brewery { Id = 1, Name = "Brewery 1"},
				new Brewery { Id = 2, Name = "Brewery 2"}
			};
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_breweryRepositoryMock = new Mock<IGenericRepository<Brewery>>();
			_unitOfWorkMock.Setup(uwm => uwm.Repository<Brewery>()).Returns(_breweryRepositoryMock.Object);

			var myProfile = new MappingProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			_mapper = new Mapper(configuration);


		}

		[Test]
		public async Task TestCreateBrewery_ShouldReturnOneAsNewId()
		{
			//Arrange
			var command = new CreateBreweryCommand() { Name = "Brewery 1 " };

			var handler = new CreateBreweryCommandHandler(_unitOfWorkMock.Object, _mapper);

			//Act
			Result<int> result = await handler.Handle(command, default);

			//Assert
			result.Succeeded.Should().BeTrue();
		}
	}
}