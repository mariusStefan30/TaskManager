using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using System.Threading.Tasks;
using System.Linq;

namespace TaskManager.Tests
{
	public class TaskServiceTests
	{
		private AppDbContext CreateInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase("TestDb")
				.Options;
			return new AppDbContext(options);
		}

		[Fact]
		public async Task Can_Create_And_Read_TaskItem()
		{
			// arrange
			await using var ctx = CreateInMemoryContext();
			var item = new TaskItem { Title = "T1", Description = "D1" };
			ctx.Tasks.Add(item);
			await ctx.SaveChangesAsync();

			// act
			var fetched = await ctx.Tasks.FirstAsync();

			// assert
			Assert.Equal("T1", fetched.Title);
		}
	}
}
