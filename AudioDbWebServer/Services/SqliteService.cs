using Dapper;
using Microsoft.Data.Sqlite;

namespace AudioDbWebServer.Services
{
	public interface ISqliteService
	{
		SqliteConnection Connection { get; }
	}

	public sealed class SqliteService : IDisposable, ISqliteService
	{
		public SqliteConnection Connection { get; private set; }

		public SqliteService(string dbPath)
		{
			var path = Path.GetFullPath(dbPath);
			var connectString = "Data Source=" + Path.GetFullPath(dbPath);

			if (File.Exists(path))
				Connection = new SqliteConnection(connectString);
			else
			{
				File.Create(path);
				Connection = new SqliteConnection(connectString);
				InitModel(Connection);
			}
		}

		private static void InitModel(SqliteConnection connection)
		{
			connection.ExecuteAsync($@"
				
			");
		}

		public void Dispose()
		{
			Connection?.Dispose();
		}
	}
}