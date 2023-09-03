using System.Reflection;
using Assessment.Domain.Common;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Assessment.Domain.Common.Interfaces;

namespace Assessment.Persistence.Contexts
{
	public class ApplicationDbContext : DbContext
	{
		private readonly IDomainEventDispatcher _dispatcher;

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
		  IDomainEventDispatcher dispatcher)
			: base(options)
		{
			_dispatcher = dispatcher;
		}

		#region DbSets
		public DbSet<Beer> Beers => Set<Beer>();
		public DbSet<Client> Clients => Set<Client>();
		public DbSet<Brewery> Breweries => Set<Brewery>();
		public DbSet<Wholesaler> Wholesalers => Set<Wholesaler>();
		public DbSet<Transaction> Transactions => Set<Transaction>();
		public DbSet<BreweryStock> BreweryStocks => Set<BreweryStock>();
		public DbSet<WholesalerSale> WholesalerSales => Set<WholesalerSale>();
		public DbSet<WholesalerStock> WholesalerStocks => Set<WholesalerStock>();
		#endregion

		protected void Seeder(ModelBuilder modelBuilder)
		{
			#region Wholesaler GeneDrinks
			int WholesalerId = 1;
			int WholesalerStockId = 1;

			modelBuilder.Entity<Wholesaler>().HasData(
				new Wholesaler()
				{
					Id = WholesalerId,
					Name = "GeneDrinks",
				});
			#endregion

			#region Abbaye de Leffe Brewery
			int BeerId = 1;
			int BrewerId = 1;
			int BreweryStockId = 1;

			modelBuilder.Entity<Brewery>().HasData(
				new Brewery()
				{
					Id = BrewerId,
					Name = "Abbaye de Leffe",
				});

			modelBuilder.Entity<Beer>().HasData(
				new Beer()
				{
					Id = BeerId,
					Price = 2.20f,
					Name = "Leffe Blonde",
					AlchoholPercentage = 6.6f,
					BreweryStockId = BreweryStockId,
				});

			modelBuilder.Entity<BreweryStock>().HasData(
				new BreweryStock()
				{
					Count = 90,
					BeerId = BeerId,
					Id = BreweryStockId,
					BreweryId = BrewerId,
				});
			#endregion

			#region Wholesaler GeneDrinks Stock
			modelBuilder.Entity<WholesalerStock>().HasData(
				new WholesalerStock()
				{
					Count = 10,
					BeerId = BeerId,
					Id = WholesalerStockId,
					WholesalerId = WholesalerId
				});
			#endregion

			#region Wholesaler Purchase
			modelBuilder.Entity<Transaction>().HasData(
				new Transaction()
				{
					Id = 1,
					Quantity = 10,
					BeerId = BeerId,
					BreweryId = BrewerId,
					WholesalerId = WholesalerId
				});
			#endregion

			#region Bières de Chimay Brewery
			BeerId = 2;
			BrewerId = 2;
			BreweryStockId = 2;

			modelBuilder.Entity<Brewery>().HasData(
				new Brewery()
				{
					Id = BrewerId,
					Name = "Bières de Chimay",
				});

			#region Chimay Bleue (Blue Cap)
			modelBuilder.Entity<Beer>().HasData(
				new Beer()
				{
					Id = BeerId,
					Price = 58.38f,
					AlchoholPercentage = 9.0f,
					BreweryStockId = BreweryStockId,
					Name = "Chimay Bleue (Blue Cap)"
				});

			modelBuilder.Entity<BreweryStock>().HasData(
				new BreweryStock()
				{
					Count = 50,
					BeerId = BeerId,
					Id = BreweryStockId,
					BreweryId = BrewerId
				});
			#endregion

			#region Chimay Dorée (Gold)
			BeerId = 3;
			BreweryStockId = 3;

			modelBuilder.Entity<Beer>().HasData(
				new Beer()
				{
					Id = BeerId,
					Price = 2.10f,
					AlchoholPercentage = 4.8f,
					Name = "Chimay Dorée (Gold)",
					BreweryStockId = BreweryStockId,
				});

			modelBuilder.Entity<BreweryStock>().HasData(
				new BreweryStock()
				{
					Count = 20,
					BeerId = BeerId,
					Id = BreweryStockId,
					BreweryId = BrewerId
				});
			#endregion

			#region Chimay Rouge (Red Cap)
			BeerId = 4;
			BreweryStockId = 4;

			modelBuilder.Entity<Beer>().HasData(
				new Beer()
				{
					Id = BeerId,
					Price = 2.10f,
					AlchoholPercentage = 7.0f,
					BreweryStockId = BreweryStockId,
					Name = "Chimay Rouge (Red Cap)",
				});

			modelBuilder.Entity<BreweryStock>().HasData(
				new BreweryStock()
				{
					Count = 30,
					BeerId = BeerId,
					Id = BreweryStockId,
					BreweryId = BrewerId
				});
			#endregion

			#region Chimay Triple (White Cap)
			BeerId = 5;
			BreweryStockId = 5;

			modelBuilder.Entity<Beer>().HasData(
				new Beer()
				{
					Id = BeerId,
					Price = 2.20f,
					AlchoholPercentage = 8.0f,
					BreweryStockId = BreweryStockId,
					Name = "Chimay Triple (White Cap)",
				});

			modelBuilder.Entity<BreweryStock>().HasData(
				new BreweryStock()
				{
					Count = 50,
					BeerId = BeerId,
					Id = BreweryStockId,
					BreweryId = BrewerId
				});
			#endregion
			#endregion

			#region Client James Peeters
			var ClientId = 1;
			modelBuilder.Entity<Client>().HasData(
				new Client()
				{
					Id = ClientId,
					Name = "James Peeters"
				});
			#endregion
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			#region Client Entities Configuration
			modelBuilder.Entity<Client>().ToTable("Client");
			modelBuilder.Entity<Client>().HasKey(c => c.Id);
			modelBuilder.Entity<Client>().HasAlternateKey(c => c.Name);
			modelBuilder.Entity<Client>().Property(c => c.Id).ValueGeneratedOnAdd();
			#endregion

			#region Brewery Entities Configuration
			#region Brewery Entity Configuration
			modelBuilder.Entity<Brewery>().ToTable("Brewery");
			modelBuilder.Entity<Brewery>().HasKey(br => br.Id);
			modelBuilder.Entity<Brewery>().HasAlternateKey(br => br.Name);
			modelBuilder.Entity<Brewery>().Property(br => br.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Brewery>().HasMany(br => br.Sales).WithOne(b => b.Brewery);
			modelBuilder.Entity<Brewery>().HasMany(br => br.Stocks).WithOne(wss => wss.Brewery);
			#endregion

			#region Beer Entity Configuration
			modelBuilder.Entity<Beer>().ToTable("Beer");
			modelBuilder.Entity<Beer>().HasKey(b => new { b.Id });
			modelBuilder.Entity<Beer>().HasAlternateKey(br => br.Name);
			modelBuilder.Entity<Beer>().Property(b => b.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Beer>().HasOne(b => b.BreweryStock).WithOne(br => br.Beer).HasForeignKey<BreweryStock>(b => b.BeerId);
			#endregion

			#region Brewery Stock Entity Configuration
			modelBuilder.Entity<BreweryStock>().ToTable("BreweryStock");
			modelBuilder.Entity<BreweryStock>().Property(br => br.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<BreweryStock>().HasOne(bs => bs.Brewery).WithMany(ws => ws.Stocks);
			modelBuilder.Entity<BreweryStock>().HasOne(bs => bs.Beer).WithOne(ws => ws.BreweryStock);
			modelBuilder.Entity<BreweryStock>().HasKey(bs => new { bs.Id, bs.BeerId, bs.BreweryId });
			#endregion

			#region Transaction Entity Configuration
			modelBuilder.Entity<Transaction>().ToTable("Transaction");
			modelBuilder.Entity<Transaction>().Property(t => t.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Transaction>().HasOne(t => t.Brewery).WithMany(b => b.Sales);
			modelBuilder.Entity<Transaction>().HasOne(t => t.WholeSaler).WithMany(b => b.Purchase);
			modelBuilder.Entity<Transaction>().HasKey(t => new { t.Id, t.BeerId, t.BreweryId, t.WholesalerId });
			#endregion
			#endregion

			#region Wholesaler Entities Configuration
			#region Wholesaler Entity Configuration
			modelBuilder.Entity<Wholesaler>().HasKey(ws => ws.Id);
			modelBuilder.Entity<Wholesaler>().ToTable("Wholesaler");
			modelBuilder.Entity<Wholesaler>().HasAlternateKey(br => br.Name);
			modelBuilder.Entity<Wholesaler>().Property(ws => ws.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<Wholesaler>().HasMany(ws => ws.Purchase).WithOne(b => b.WholeSaler);
			modelBuilder.Entity<Wholesaler>().HasMany(ws => ws.Stocks).WithOne(wss => wss.Wholesaler);
			#endregion

			#region Wholesaler Stock Entity Configuration
			modelBuilder.Entity<WholesalerStock>().ToTable("WholesalerStock");
			modelBuilder.Entity<WholesalerStock>().Property(wss => wss.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<WholesalerStock>().HasKey(wss => new { wss.Id, wss.WholesalerId });
			modelBuilder.Entity<WholesalerStock>().HasOne(wss => wss.Beer).WithMany(ws => ws.Stocks);
			modelBuilder.Entity<WholesalerStock>().HasOne(wss => wss.Wholesaler).WithMany(ws => ws.Stocks);
			#endregion

			#region Wholesaler Sales Entity Configuration
			modelBuilder.Entity<WholesalerSale>().ToTable("WholesalerSales");
			modelBuilder.Entity<WholesalerSale>().HasOne(wss => wss.Wholesaler).WithMany(ws => ws.Sales);
			modelBuilder.Entity<WholesalerSale>().HasOne(wss => wss.Client).WithMany(ws => ws.Purchases);
			modelBuilder.Entity<WholesalerSale>().HasOne(wss => wss.SoldBeer).WithMany(ws => ws.WholesalerSales);
			modelBuilder.Entity<WholesalerSale>().HasKey(wss => new { wss.Id, wss.WholesalerId, wss.BeerId, wss.ClientId });
			#endregion
			#endregion

			Seeder(modelBuilder);
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			var entries = ChangeTracker
				.Entries()
				.Where(e => e.Entity is BaseEntity && (
						e.State == EntityState.Added
						|| e.State == EntityState.Modified));

			foreach (var entityEntry in entries)
			{
				((BaseAuditableEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

				if (entityEntry.State == EntityState.Added)
				{
					((BaseAuditableEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
				}
			}

			int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			// ignore events if no dispatcher provided
			if (_dispatcher == null) return result;

			// dispatch events only if save was successful
			var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
				.Select(e => e.Entity)
				.Where(e => e.DomainEvents.Any())
				.ToArray();

			await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

			return result;
		}

		public override int SaveChanges()
		{
			return SaveChangesAsync().GetAwaiter().GetResult();
		}
	}
}
