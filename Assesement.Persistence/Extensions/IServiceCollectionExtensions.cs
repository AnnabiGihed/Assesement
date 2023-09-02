using Microsoft.EntityFrameworkCore;
using Assesement.Persistence.Contexts;
using Microsoft.Extensions.Configuration;
using Assesement.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Assesement.Application.Interfaces.Repositories;

namespace Assesement.Persistence.Extensions
{
	public static class IServiceCollectionExtensions
	{
		public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext(configuration);
			services.AddRepositories();
		}

		public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<ApplicationDbContext>(options =>
			   options.UseSqlServer(connectionString,
				   builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
		}

		private static void AddRepositories(this IServiceCollection services)
		{
			services
				.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
				.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
				.AddTransient<IBeerRepository, BeerRepository>()
				.AddTransient<IClientRepository, ClientRepository>()
				.AddTransient<IBreweryRepository, BreweryRepository>()
				.AddTransient<IWholesalerRepository, WholesalerRepository>()
				.AddTransient<ITransactionRepository, TransactionRepository>()
				.AddTransient<IBreweryStockRepository, BreweryStockRepository>()
				.AddTransient<IWholesalerSaleRepository, WholesalerSaleRepository>()
				.AddTransient<IWholesalerStockRepository, WholesalerStockRepository>();
		}
	}
}
