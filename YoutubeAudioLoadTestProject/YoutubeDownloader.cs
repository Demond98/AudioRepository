using System.Reflection;
using Xabe.FFmpeg;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeAudioLoadTestProject
{
	public class YoutubeDownloader
	{
		private const int BatchSize = 5;
		
		private readonly YoutubeClient _youtubeClient;

		public YoutubeDownloader(YoutubeClient youtubeClient)
		{
			_youtubeClient = youtubeClient;
		}

		public async Task DownloadVideos(string playListAddress)
		{
			var videos = await _youtubeClient.Playlists.GetVideosAsync(playListAddress);

			var streamInfoTasks = videos.Select(async v => await GetStreamInfo(v)).ToList();
			await Task.WhenAll(streamInfoTasks);
			var streamInfos = streamInfoTasks.Select(t => t.Result).ToList();

			var downloadTasks = streamInfos.Select(async t => await DownloadVideo(t.streamInfo, t.video)).ToList();
			await BatchExecutor.Execute(downloadTasks, BatchSize);

			var videoNames = streamInfos.Select(t => t.video).ToList();

			foreach (var videoName in videoNames)
				await ConvertAudio(videoName);
		}

		private async Task<(IStreamInfo streamInfo, string video)> GetStreamInfo(IVideo video)
		{
			var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(video.Id);
			
			var muxedStreamInfos = streamManifest
				.GetMuxedStreams()
				.Where(v => v.Container == Container.Mp4)
				.ToList();
			
			var lowestSize = muxedStreamInfos.Min(x => x.Size);
			var streamInfo = muxedStreamInfos
				.Where(s => s.Size == lowestSize)
				.GetWithHighestBitrate();
			
			return (streamInfo, video.Title);
		}

		private async Task DownloadVideo(IStreamInfo streamInfo, string videoName)
		{
			var convertedName = videoName
				.Replace(" ", string.Empty)
				.Replace("!", string.Empty);
				
			var videoFile = $"tempVideos/{convertedName}.mp4";

			await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, videoFile);
		}

		private static async Task ConvertAudio(string videoName)
		{
			var convertedName = videoName
				.Replace(" ", string.Empty)
				.Replace("!", string.Empty);
			
			var videoFile = $"tempVideos/{convertedName}.mp4";
			var audioFile = $"audios/{convertedName}.mp3";

			var snippets = CreateSnippets();
			var conversion = await snippets.ExtractAudio(videoFile, audioFile);
			await conversion.Start();
		}

		private static Snippets CreateSnippets()
		{
			//TODOD: Говнокод 80 lvl, потому что они почему-то сделали конструктор внутренним, а как создать я хз
			return (Snippets) typeof(Snippets).GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance,
				null, Type.EmptyTypes, null)!.Invoke(null);
		}
	}
}