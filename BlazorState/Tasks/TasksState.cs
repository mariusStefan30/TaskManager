// "Singura sursă de adevăr" pentru UI în Fluxor.
using Fluxor;
using TaskManager.Models.TaskDatatransferobject;

namespace TaskManager.BlazorState.Tasks
{
	// Declară acest state ca feature; Fluxor îl va crea și injecta.
	[FeatureState]
	public record TasksState
	{
		// Indică dacă încărcăm datele din API
		public bool IsLoading { get; init; }

		// Mesaj de eroare (dacă a eșuat o acțiune)
		public string? Error { get; init; }

		// Lista de task-uri pentru UI
		public IReadOnlyList<TaskDto> Items { get; init; }

		// Fluxor cere un ctor fără parametri — setăm valorile implicite.
		public TasksState() : this(false, null, Array.Empty<TaskDto>()) { }

		// Ctor util când vrem să construim state-ul manual.
		public TasksState(bool isLoading, string? error, IReadOnlyList<TaskDto> items)
		{
			IsLoading = isLoading;
			Error = error;
			Items = items;
		}
	}
}

