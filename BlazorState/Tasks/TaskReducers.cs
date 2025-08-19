// Funcții PURE (fără side-effects) care transformă (state, action) -> noul state.
using Fluxor;
using TaskManager.Models.TaskDatatransferobject;

namespace TaskManager.BlazorState.Tasks
{
	public static class TaskReducers
	{
		// 1) Load -> punem loading=true și curățăm eroarea
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, LoadTasksAction _)
			=> state with { IsLoading = true, Error = null };

		// 2) LoadSuccess -> punem itemele și loading=false
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, LoadTasksSuccessAction action)
			=> state with { IsLoading = false, Error = null, Items = action.Items };

		// 3) LoadFailure -> afișăm eroarea și oprim loading
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, LoadTasksFailureAction action)
			=> state with { IsLoading = false, Error = action.Error };

		//Adaugam create task reducer for the create task actions
		//Optimistic start: setam doar loading
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, CreateTaskAction _)
			=> state with { IsLoading = true, Error = null };

		//Dupa raspunsul API adaugam itemul in lista
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, CreateTaskSuccessAction action)
			=> state with
			{
				IsLoading = false,
				Items = state.Items.Append(action.Created).ToArray(),
				Error = null
			};

		[ReducerMethod]
		public static TasksState Reduce(TasksState state, CreateTaskFailureAction action)
			=> state with { IsLoading = false, Error = action.Error };

		// 4) Optimistic toggle (UI răspunde imediat, efectul va confirma)
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, ToggleDoneAction action)
			=> state with
			{
				Items = state.Items
					.Select(x => x.Id == action.Id ? x with { IsDone = action.IsDone } : x)
					.ToArray()
			};

		// 5) Confirmarea de la server pentru update (de ex. alt câmp a fost schimbat)
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, UpdateTaskSuccessAction action)
			=> state with
			{
				Items = state.Items
					.Select(x => x.Id == action.Updated.Id ? action.Updated : x)
					.ToArray()
			};

		// 6) Delete confirmat
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, DeleteTaskSuccessAction action)
			=> state with
			{
				Items = state.Items
					.Where(x => x.Id != action.Id)
					.ToArray()
			};

		// 7) Add confirmat (adăugăm elementul nou)
		[ReducerMethod]
		public static TasksState Reduce(TasksState state, AddTaskSuccessAction action)
			=> state with { Items = state.Items.Append(action.Created).ToArray() };
	}
}
