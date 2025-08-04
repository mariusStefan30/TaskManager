using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManager.Controllers;
using TaskManager.Models;
using TaskManager.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Tests
{
	public class TaskItemsControllerTests
	{
		private AppDbContext CreateInMemoryContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase("TestDb_Controllers")
				.Options;
			return new AppDbContext(options);
		}

		[Fact]
		public async Task GetTasks_Returms_AllTasks()
		{
			//Arrange
			using var context = CreateInMemoryContext();
			context.Tasks.AddRange(
				new TaskItem { Title = "T1" , Description = "D1" },
				new TaskItem { Title = "T2", Description = "D2"}
			);
			await context.SaveChangesAsync();

			var controller = new TaskItemsController( context );

			//Act
			var result = await controller.Index(null);

			//Assert
			var viewResult = Assert.IsType<ViewResult>( result );
			var model = Assert.IsType<List<TaskItem>>(viewResult.Model);
			Assert.Equal( 2, model.Count );
		}
	}
}
