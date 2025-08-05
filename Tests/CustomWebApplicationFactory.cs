using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.Data;

namespace TaskManager.Tests
{
	public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
	{
		private readonly string _dbName = Guid.NewGuid().ToString();

		protected override void ConfigureWebHost(IWebHostBuilder builder) 
		{
			builder.ConfigureServices(services =>
			{
				//  Golește complet toate înregistrările DbContext existente
				var toRemove = services
					.Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>))
					.ToList();

				foreach (var item in toRemove)
					services.Remove(item);

				//  Adaugă InMemory în loc
				services.AddDbContext<AppDbContext>(options =>
				{
					options.UseInMemoryDatabase(_dbName);
				});

				//  Fără EnsureDeleted (pentru stabilitate)
				// Doar EnsureCreated pentru schema
				var sp = services.BuildServiceProvider();
				using (var scope = sp.CreateScope())
				{
					var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
					db.Database.EnsureCreated();
				}


				//refactor above
				//// eliminam contexul SQL real
				//var descriptor = services.SingleOrDefault(
				//	d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
				//);
				//if (descriptor != null)
				//{
				//	services.Remove( descriptor );
				//}

				//var contextDescriptor = services.SingleOrDefault(
				//d => d.ServiceType == typeof(AppDbContext));
				//if (contextDescriptor != null)
				//	services.Remove(contextDescriptor);

				////adaugam EF InMemory cu un nume unic
				//services.AddDbContext<AppDbContext>(options =>
				//{
				//	options.UseInMemoryDatabase( _dbName );
				//});

				////seed / resetare
				//var sp = services.BuildServiceProvider();
				//using (var scope = sp.CreateScope())
				//{
				//	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				//	db.Database.EnsureDeleted();  // Cleanup
				//	db.Database.EnsureCreated();  // Schema recreate
				//}
			});
		}
	}
}
