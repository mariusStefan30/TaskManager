// "Mesaje" simple care descriu ce s-a întâmplat în UI.
using TaskManager.Models.TaskDatatransferobject;

namespace TaskManager.BlazorState.Tasks
{
	// Cerem încărcarea tuturor task-urilor
	public record LoadTasksAction;

	// Încărcarea a reușit -> trimitem elementele
	public record LoadTasksSuccessAction(IReadOnlyList<TaskDto> Items);

	// Încărcarea a eșuat -> trimitem mesajul de eroare
	public record LoadTasksFailureAction(string Error);

	// User a bifat/debifat "IsDone" în UI (optimistic update în reducer)
	public record ToggleDoneAction(int Id, bool IsDone);

	// Serverul a confirmat update-ul cu versiunea finală
	public record UpdateTaskSuccessAction(TaskDto Updated);

	// Cerere de ștergere
	public record DeleteTaskAction(int Id);

	// Serverul a confirmat ștergerea
	public record DeleteTaskSuccessAction(int Id);

	// Cerere de creare
	public record AddTaskAction(TaskDto NewItem);

	// Serverul a confirmat crearea (cu Id real)
	public record AddTaskSuccessAction(TaskDto Created);
}
