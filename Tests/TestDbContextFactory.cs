using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

namespace TaskManager.Tests
{
	public class TestDbContextFactory
	{
		public static AppDbContext CreateInMemoryContext(string dbName)
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: dbName)
				.Options;

			var context = new AppDbContext(options);
			context.Database.EnsureDeleted(); // cleanup total
			context.Database.EnsureCreated(); // recreate schema

			return context;
		}
	}
}
