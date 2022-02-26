namespace AudioUploader._Common.Extensions
{
	public static class BatchManager
	{
		public static async Task Execute(IEnumerable<Task> tasks, int batchSize)
		{
			var executingTasks = new Task[batchSize];

			using var enumerator = tasks.GetEnumerator();

			for (var id = 0; id < batchSize && enumerator.MoveNext(); id++)
				executingTasks[id] = enumerator.Current;

			while (enumerator.MoveNext())
			{
				await Task.WhenAny(executingTasks);
				var completedTaskId = Task.WaitAny(executingTasks);
				executingTasks[completedTaskId] = enumerator.Current;
			}

			await Task.WhenAll(executingTasks.Where(t => t != null));
		}

		public static async IAsyncEnumerable<T> ExecuteWithResult<T>(IEnumerable<Task<T>> tasks, int batchSize)
		{
			var executingTasks = new Task<T>[batchSize];

			using var enumerator = tasks.GetEnumerator();

			for (var id = 0; id < batchSize && enumerator.MoveNext(); id++)
				executingTasks[id] = enumerator.Current;

			while (enumerator.MoveNext())
			{
				await Task.WhenAny(executingTasks);
				var completedTaskId = Task.WaitAny(executingTasks);
				yield return executingTasks[completedTaskId].Result;
				executingTasks[completedTaskId] = enumerator.Current;
			}

			var finalExecutingTasks = executingTasks.Where(t => t != null).ToArray();
			await Task.WhenAll(finalExecutingTasks);
			foreach (var task in finalExecutingTasks)
				yield return task.Result;
		}
	}
}
