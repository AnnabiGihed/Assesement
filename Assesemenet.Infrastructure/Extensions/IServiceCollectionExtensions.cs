using MediatR;
using Assesement.Domain.Common;
using Assesement.Application.Interfaces;
using Assesement.Domain.Common.Interfaces;
using Assesemenet.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Assesemenet.Infrastructure.Extensions
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
				.AddTransient<IDateTimeService, DateTimeService>()
		}
	}
}
