using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using Microsoft.Extensions.Logging.Abstractions;


namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<TaskItemsApiController> _logger;

		public TaskItemsApiController(AppDbContext db, ILogger<TaskItemsApiController>? logger = null)
		{
			_db = db;
			_logger = logger ?? NullLogger<TaskItemsApiController>.Instance;
		}
		// GET: api/TaskItemsApi
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
		{
			_logger.LogInformation("GET all tasks requested");
			var tasks = await _db.Tasks.ToListAsync();
			_logger.LogInformation("Returned {Count} tasks", tasks.Count);
			return tasks;
		}


		// GET: api/TaskItemsApi/5
		[HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> Get(int id)
        {
			_logger.LogInformation("GET task by ID: {TaskId}", id);
			var item = await _db.Tasks.FindAsync(id);
			if (item == null)
			{
				_logger.LogWarning("Task with ID {TaskId} not found", id);
				return NotFound();
			}

			return item;
		}

        // POST: api/TaskItemsApi
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem dto)
        {
			_logger.LogInformation("POST: creating new task with title '{Title}'", dto.Title);
			_db.Tasks.Add(dto);
            await _db.SaveChangesAsync();
			_logger.LogInformation(" Task created with ID {TaskId}", dto.Id);
			return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // PUT: api/TaskItemsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem dto)
        {
			if (id != dto.Id)
			{
				_logger.LogWarning("PUT: ID mismatch (route ID: {RouteId}, body ID: {BodyId})", id, dto.Id);
				return BadRequest();
			}
			_logger.LogInformation("PUT: updating task ID {TaskId}", id);

			try
			{
				await _db.SaveChangesAsync();
				_logger.LogInformation(" Task ID {TaskId} updated successfully", id);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await _db.Tasks.AnyAsync(t => t.Id == id))
				{
					_logger.LogWarning("PUT: Task ID {TaskId} not found during update", id);
					return NotFound();
				}
				else
				{
					_logger.LogError("PUT: concurrency error while updating task ID {TaskId}", id);
					throw;
				}
			}

			return NoContent();
		}

        // DELETE: api/TaskItemsApi/5
        [Authorize(Policy = "RequireManagerOrAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
			_logger.LogInformation(" DELETE request for task ID {TaskId}", id);
			var item = await _db.Tasks.FindAsync(id);

			if (item == null)
			{
				_logger.LogWarning(" DELETE: Task with ID {TaskId} not found", id);
				return NotFound();
			}

			_db.Tasks.Remove(item);
			await _db.SaveChangesAsync();
			_logger.LogInformation(" Task ID {TaskId} deleted", id);

			return NoContent();
		}


    }
}
