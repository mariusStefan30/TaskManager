// Folosim un "Data Transfer Object" imutabil pentru UI și API.
namespace TaskManager.Models.TaskDatatransferobject
{
	// record = tip imutabil; putem folosi sintaxa `with` în reducer-e.
	public record TaskDto(
		int Id,                // identificatorul task-ului
		string Title,          // titlul
		string? Description,   // descrierea (poate lipsi => string?)
		bool IsDone            // statusul "finalizat?"
	);
}

