// Efecte = side-effects: HTTP către API, apoi Dispatch(...) acțiuni de Success/Failure.
using System.Net.Http.Json;
using Fluxor;
using TaskManager.Models.TaskDatatransferobject;
using Microsoft.AspNetCore.Components;

namespace TaskManager.BlazorState.Tasks
{
	public class TaskEffects
	{
		private readonly HttpClient _http;        // pentru apelurile la API
        private readonly IState<TasksState> _state; // putem citi state-ul curent (ex. pentru Toggle)
		private readonly NavigationManager _nav;


		public TaskEffects(HttpClient http, IState<TasksState> state, NavigationManager nav)
		{
			_http = http;
			_state = state;
			_nav = nav;
		}

		// 1) Efect pentru LoadTasksAction
		[EffectMethod]
		public async Task Handle(LoadTasksAction action, IDispatcher dispatcher)
		{
			try
			{
				// GET /api/TaskItemsApi -> listă de TaskDto
				var items = await _http.GetFromJsonAsync<List<TaskDto>>("api/TaskItemsApi");
				dispatcher.Dispatch(new LoadTasksSuccessAction(items ?? new List<TaskDto>()));
			}
			catch (Exception ex)
			{
				dispatcher.Dispatch(new LoadTasksFailureAction(ex.Message));
			}
		}

		//Efect pentru CreateTaskAction
		[EffectMethod]
		public async Task Handle(CreateTaskAction action, IDispatcher dispatcher)
		{
            try
            {
                // pe Server nu ai mereu BaseAddress – seteaz-o din site
                _http.BaseAddress ??= new Uri(_nav.BaseUri);

                var resp = await _http.PostAsJsonAsync("api/TaskItemsApi", action.NewTask);
                resp.EnsureSuccessStatusCode();

                var created = await resp.Content.ReadFromJsonAsync<TaskDto>(
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                dispatcher.Dispatch(new CreateTaskSuccessAction(created!));
                dispatcher.Dispatch(new LoadTasksAction());
                _nav.NavigateTo("/tasks");
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new CreateTaskFailureAction(ex.Message));
            }
        }

		// 2) Efect pentru ToggleDoneAction -> PUT către API
		[EffectMethod]
		public async Task Handle(ToggleDoneAction action, IDispatcher dispatcher)
		{
			// Găsim în state task-ul curent
			var current = _state.Value.Items.FirstOrDefault(x => x.Id == action.Id);
			if (current is null) return;

			// Construim versiunea actualizată (immutabil) pentru server
			var updated = current with { IsDone = action.IsDone };

			// PUT /api/TaskItemsApi/{id} cu DTO-ul complet
			var resp = await _http.PutAsJsonAsync($"api/TaskItemsApi/{action.Id}", updated);
			if (resp.IsSuccessStatusCode)
			{
				// Anunțăm reducer-ele că serverul a confirmat update-ul
				dispatcher.Dispatch(new UpdateTaskSuccessAction(updated));
			}
			else
			{
				// Dacă vrei, poți reîncărca lista ca să "repari" optimistic update-ul
				dispatcher.Dispatch(new LoadTasksAction());
			}
		}

		// 3) Efect pentru DeleteTaskAction -> DELETE
		[EffectMethod]
		public async Task Handle(DeleteTaskAction action, IDispatcher dispatcher)
		{
			var resp = await _http.DeleteAsync($"api/TaskItemsApi/{action.Id}");
			if (resp.IsSuccessStatusCode)
				dispatcher.Dispatch(new DeleteTaskSuccessAction(action.Id));
			else
				dispatcher.Dispatch(new LoadTasksAction()); // fallback la refresh
		}

		// 4) Efect pentru AddTaskAction -> POST, apoi AddTaskSuccessAction
		[EffectMethod]
		public async Task Handle(AddTaskAction action, IDispatcher dispatcher)
		{
			var resp = await _http.PostAsJsonAsync("api/TaskItemsApi", action.NewItem);
			if (resp.IsSuccessStatusCode)
			{
				var created = await resp.Content.ReadFromJsonAsync<TaskDto>(
					new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (created is not null)
					dispatcher.Dispatch(new AddTaskSuccessAction(created));
				else
					dispatcher.Dispatch(new LoadTasksAction());
			}
			else
			{
				dispatcher.Dispatch(new LoadTasksAction());
			}
		}
	}
}
