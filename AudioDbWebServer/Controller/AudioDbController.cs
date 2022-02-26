using AudioDbWebServer.Model;
using AudioDbWebServer.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AudioDbWebServer.Controller
{
	[ApiController]
	public class AudioDbController : ControllerBase
	{
		public ISqliteService Db { get; set; }

		public AudioDbController(ISqliteService db)
		{
			Db = db;
		}		

		[HttpGet("audios")]
		public async Task<IEnumerable<Audio>> GetAudios()
		{
			return await Db.Connection.QueryAsync<Audio>(@"SELECT * FROM Audios");
		}

		[HttpGet("audios/{id}")]
		public async Task<Audio> GetAudio(int id)
		{
			return await Db.Connection.QuerySingleAsync<Audio>(@$"SELECT * FROM Audios WHERE Id = {id} LIMIT 1");
		}
	}
}
