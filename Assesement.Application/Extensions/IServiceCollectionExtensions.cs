using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Assesement.Application.Extensions
{
	public static class IServiceCollectionExtensions
	{
		public static void AddApplicationLayer(this IServiceCollection services)
		{
			services.AddMediator();
			services.AddAutoMapper();
			services.AddValidators();
		}

		private static void AddAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
		}

		private static void AddMediator(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		}

		private static void AddValidators(this IServiceCollection services)
		{
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
