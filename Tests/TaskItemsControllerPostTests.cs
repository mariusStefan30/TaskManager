using Xunit;
using TaskManager.Controllers;
using TaskManager.Data;
using TaskManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TaskManager.Tests
{
	public class TaskItemsControllerPostTests
	{
		private AppDbContext CreateInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase("TestDb_Post")
				.Options;

			return new AppDbContext(options);
		}

		[Fact]
		public async Task Create_Post_Adds_Task_And_Redirects()
		{
			// Arrange
			using var context = CreateInMemoryContext();
			var controller = new TaskItemsController(context);
			var newTask = new TaskItem { Title = "T_POST", Description = "D_POST" };

			// Act
			var result = await controller.Create(newTask);

			// Assert
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirect.ActionName);

			var taskInDb = await context.Tasks.FirstOrDefaultAsync(t => t.Title == "T_POST");
			Assert.NotNull(taskInDb);
			Assert.Equal("D_POST", taskInDb.Description);
		}
	}
}
