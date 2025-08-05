using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Models;
using Xunit;
using TaskManager.Data;

namespace TaskManager.Tests
{
	public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		//private readonly CustomWebApplicationFactory<Program> _factoryTest;

		public ApiIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_client = factory.CreateClient();
		}
		//public ApiIntegrationTests(CuystomWebApplicationFactory<Program> factoryTest)
		//{
	
		//	_factoryTest = factoryTest;
		//}


		[Fact]
		public async Task GetAll_ReturnsOkAndEmptyListInitially()
		{
			//// La începutul fiecărui test // inca nu fucntioneaza sa fac cleanup manual în fiecare test
			//var scope = factoryTest.Services.CreateScope();
			//var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			//db.Database.EnsureDeleted();
			//db.Database.EnsureCreated();

			// Act
			var response = await _client.GetAsync("/api/TaskItemsApi");

			// Assert
			response.EnsureSuccessStatusCode(); // 200
			var json = await response.Content.ReadAsStringAsync();
			Assert.Contains("[", json); // sau deserializează lista și verifici .Count == 0
		}

		[Fact]
		public async Task Post_CreatesTaskItem()
		{
	
			// Arrange
			var newTask = new TaskItem
			{
				Title = "Test POST",
				Description = "Post description"
			};

			var content = new StringContent(JsonSerializer.Serialize(newTask), Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/TaskItemsApi", content);

			// Assert
			response.EnsureSuccessStatusCode(); // 201 Created
			var responseBody = await response.Content.ReadAsStringAsync();
			var createdItem = JsonSerializer.Deserialize<TaskItem>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			Assert.Equal("Test POST", createdItem.Title);
		}

		[Fact]
		public async Task Put_UpdatesExistingTask()
		{

		

			// Arrange - adăugăm un task mai întâi
			var task = new TaskItem { Title = "Initial", Description = "To be updated" };
			var postContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
			var postResp = await _client.PostAsync("/api/TaskItemsApi", postContent);
			var responseBody = await postResp.Content.ReadAsStringAsync();
			var created = JsonSerializer.Deserialize<TaskItem>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			// Modificăm task-ul
			created.Title = "Updated title";
			var putContent = new StringContent(JsonSerializer.Serialize(created), Encoding.UTF8, "application/json");

			// Act
			var putResp = await _client.PutAsync($"/api/TaskItemsApi/{created.Id}", putContent);

			// Assert
			Assert.Equal(HttpStatusCode.NoContent, putResp.StatusCode);
		}

		[Fact]
		public async Task Delete_RemovesTaskItem()
		{

		

			// Arrange - creează mai întâi task
			var task = new TaskItem { Title = "To be deleted", Description = "Delete me" };
			var content = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
			var postResp = await _client.PostAsync("/api/TaskItemsApi", content);
			var responseBody = await postResp.Content.ReadAsStringAsync();
			var created = JsonSerializer.Deserialize<TaskItem>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			// Act - ștergem
			var deleteResp = await _client.DeleteAsync($"/api/TaskItemsApi/{created.Id}");

			// Assert
			Assert.Equal(HttpStatusCode.NoContent, deleteResp.StatusCode);
		}


		//Refactor above
		//[Fact]
		//public async Task GetAllTasks_ReturnsListOfTasks()
		//{
		//	// Act
		//	var response = await _client.GetAsync("/api/TaskItemsApi");

		//	// Assert
		//	response.EnsureSuccessStatusCode();

		//	var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>();

		//	Assert.NotNull(tasks);
		//	Assert.All(tasks!, task =>
		//	{
		//		Assert.False(string.IsNullOrWhiteSpace(task.Title));
		//	});
		//}

		//[Fact]
		//public async Task PostTask_CreateTaskAndReturnsIt()
		//{
		//	//Arange
		//	var newTask = new TaskItem
		//	{
		//		Title = "Test POST task",
		//		Description = "Test POST description",
		//		IsDone = false
		//	};

		//	//Act
		//	var response = await _client.PostAsJsonAsync("/api/TaskItemsApi", newTask);

		//	//Assert
		//	response.EnsureSuccessStatusCode(); //201
		//	var createdtask = await response.Content.ReadFromJsonAsync<TaskItem>();

		//	Assert.NotNull(createdtask);
		//	Assert.Equal(newTask.Title, createdtask!.Title);
		//	Assert.Equal(newTask.Description, createdtask.Description);
		//}

		//[Fact]
		//public async Task PutTask_UpdatesExistingTask()
		//{
		//	//Arrange: Cream mai intai task
		//	var originalTask = new TaskItem
		//	{
		//		Title = "Initial Title",
		//		Description = "Initial Description",
		//		IsDone = false
		//	};

		//	var postResponse = await _client.PostAsJsonAsync("/api/TaskItemsApi", originalTask);
		//	var createdTask = await postResponse.Content.ReadFromJsonAsync<TaskItem>();
		//	Assert.NotNull(createdTask);

		//	//Modificam taskul
		//	createdTask!.Title = "Updated Title";
		//	createdTask.Description = "Updated Description";

		//	//Act: Apelam PUT
		//	var putResponse = await _client.PutAsJsonAsync($"/api/TaskItemsApi/{createdTask.Id}", createdTask);

		//	//Verificam ca s-a actualizar correct
		//	var getResponse = await _client.GetAsync($"/api/TaskItemsApi/{createdTask.Id}");
		//	var updatedTask = await getResponse.Content.ReadFromJsonAsync<TaskItem>();

		//	Assert.Equal("Updated Title", updatedTask!.Title);
		//	Assert.Equal("Updated Description", updatedTask.Description);	
		//}

		//[Fact]
		//public async Task DeleteTask_RemovesTask() 
		//{
		//	//Arrange : Cream task-ul
		//	var newTask = new TaskItem
		//	{
		//		Title = "Task to delete",
		//		Description = "To be deleted"
		//	};

		//	var postResponse = await _client.PostAsJsonAsync("/api/TaskItemsApi", newTask);
		//	var createdTask = await postResponse.Content.ReadFromJsonAsync<TaskItem>();

		//	Assert.NotNull(createdTask);

		//	//Act: sterge task-ul
		//	var deleteRepsonse = await _client.DeleteAsync($"/api/TaskItemsApi/{createdTask!.Id}");

		//	//Assert: Raspuns trebuie sa fie 204 NoContent
		//	Assert.Equal(HttpStatusCode.NoContent, deleteRepsonse.StatusCode);

		//	//Verificam ca nu mai exista in baza de date
		//	var getResponse = await _client.GetAsync($"api/TaskItemsApi/{createdTask.Id}");

		//	Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
		//}
	}
}
