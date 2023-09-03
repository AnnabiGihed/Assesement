using Assessment.Domain.Common;
using Assessment.Application.Interfaces;
using Assessment.Domain.Common.Interfaces;
using Assessment.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

using MediatR;
namespace Assessment.Infrastructure.Extensions
{
	public static class IServiceCollectionExtensions
	{
		public static void AddInfrastructureLayer(this IServiceCollection services)
		{
			services.AddServices();
		}

		private static void AddServices(this IServiceCollection services)
		{
			services
				.AddTransient<IMediator, Mediator>()
				.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
				.AddTransient<IDateTimeService, DateTimeService>();

		}
	}
}
