namespace YoutubeAudioLoadTestProject
{
	public static class BatchExecutor
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
	}
}
