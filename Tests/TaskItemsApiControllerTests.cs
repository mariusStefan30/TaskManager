using Xunit;
using TaskManager.Controllers;
using TaskManager.Data;
using TaskManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TaskManager.Tests
{
	public class TaskItemsApiControllerTests
	{
		private static AppDbContext CreateInMemoryContext(string dbName)
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			return new AppDbContext(options);
		}


		[Fact]
		public async Task Create_AddsTask_AndReturnsCreatedAtAction()
		{
			// Arrange
			var context = CreateInMemoryContext("TestDb_Post");
			var controller = new TaskItemsApiController(context);

			var newTask = new TaskItem { Title = "Test task", Description = "Test desc" };

			// Act
			var result = await controller.Create(newTask);

			// Assert
			var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var returnedTask = Assert.IsType<TaskItem>(createdResult.Value);

			Assert.Equal("Test task", returnedTask.Title);
			Assert.Single(context.Tasks); // verificăm că s-a salvat
		}

		[Fact]
		public async Task Get_ReturnsTaskItem_WhenFound()
		{
			// Arrange
			var context = CreateInMemoryContext("TestDb_Get");
			var task = new TaskItem { Title = "Get Test", Description = "Test desc" };
			context.Tasks.Add(task);
			await context.SaveChangesAsync();

			var controller = new TaskItemsApiController(context);

			// Act
			var result = await controller.Get(task.Id);

			// Assert
			var okResult = Assert.IsType<ActionResult<TaskItem>>(result);
			var returnedTask = Assert.IsType<TaskItem>(okResult.Value);
			Assert.Equal("Get Test", returnedTask.Title);
		}


		[Fact]
		public async Task Update_ValidTask_ReturnsNoContent() //Broken, o sa il resolv alta data
		{
			// Arrange
			var context = CreateInMemoryContext("TestDb_Put");
			var task = new TaskItem { Title = "Old Title", Description = "Old Desc" };
			context.Tasks.Add(task);
			await context.SaveChangesAsync();

			var controller = new TaskItemsApiController(context);

			// Modificăm taskul
			var updatedTask = new TaskItem
			{
				Id = task.Id,
				Title = "Updated Title",
				Description = "Updated Desc"
			};

			// Act
			var result = await controller.Update(task.Id, updatedTask);

			// Assert
			Assert.IsType<NoContentResult>(result);
			var savedTask = await context.Tasks.FindAsync(task.Id);
			Assert.Equal("Updated Title", savedTask!.Title);
		}


		[Fact]
		public async Task Update_IdMismatch_ReturnsBadRequest()
		{
			// Arrange
			var context = CreateInMemoryContext("TestDb_Put_BadRequest");
			var task = new TaskItem { Title = "Test", Description = "Test" };
			context.Tasks.Add(task);
			await context.SaveChangesAsync();

			var controller = new TaskItemsApiController(context);

			// DTO cu alt ID decât în URL
			var updatedTask = new TaskItem
			{
				Id = task.Id + 1,
				Title = "Different ID",
				Description = "Should fail"
			};

			// Act
			var result = await controller.Update(task.Id, updatedTask);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async Task Get_ReturnsNotFound_WhenTaskDoesNotExist()
		{
			// Arrange
			var context = CreateInMemoryContext("TestDb_Get_NotFound");
			var controller = new TaskItemsApiController(context);

			// Act
			var result = await controller.Get(999); // ID inexistent

			// Assert
			var actionResult = Assert.IsType<ActionResult<TaskItem>>(result);
			Assert.IsType<NotFoundResult>(actionResult.Result);
		}


	}
}
