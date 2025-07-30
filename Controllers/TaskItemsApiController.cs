using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TaskItemsApiController(AppDbContext db) => _db = db;

        // GET: api/TaskItemsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
            => await _db.Tasks.ToListAsync();

        // GET: api/TaskItemsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> Get(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        // POST: api/TaskItemsApi
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem dto)
        {
            _db.Tasks.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // PUT: api/TaskItemsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem dto)
        {
            if (id != dto.Id) return BadRequest();
            _db.Entry(dto).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/TaskItemsApi/5
        [Authorize(Policy = "RequireManagerOrAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            if (item == null) return NotFound();
            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
