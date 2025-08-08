// Extension methods pentru a converti între entitatea EF și DTO.
// => e util când vrei să întorci DTO din controller.
using TaskManager.Models;

namespace TaskManager.Models.TaskDatatransferobject
{
	public static class TaskMappings
	{
		// Entitate EF -> DTO
		public static TaskDto ToDto(this TaskItem e)
			=> new(e.Id, e.Title, e.Description, e.IsDone);

		// DTO -> entitate EF (când primești din UI un DTO și vrei să-l salvezi)
		public static TaskItem ToEntity(this TaskDto d)
			=> new TaskItem
			{
				Id = d.Id,
				Title = d.Title,
				Description = d.Description,
				IsDone = d.IsDone
			};
	}
}
