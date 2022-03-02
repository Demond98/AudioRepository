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

		[HttpGet("playlists")]
		public async Task<IEnumerable<Playlist>> GetPlaylists()
		{
			return await Db.Connection.QueryAsync<Playlist>(@$"SELECT * FROM Playlist");
		}

		[HttpGet("playlists/{id}")]
		public async Task<Playlist> GetPlaylist(int id)
		{
			return await Db.Connection.QuerySingleAsync<Playlist>(@$"SELECT * FROM Playlist WHERE Id = {id} LIMIT 1");
		}

		[HttpGet("playlists/audios/{id}")]
		public async Task<IEnumerable<Audio>> GetPlaylistAudios(int playlistAudios)
		{
			return await Db.Connection.QueryAsync<Audio>(@$"
SELECT * FROM Playlist
WHERE PlaylistId = {playlistAudios}
LEFT JOIN PlaylistAduio AS pa
ON pa.Id = Playlist.Id");
		}
	}
}