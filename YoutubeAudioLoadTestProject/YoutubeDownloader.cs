using System.Diagnostics;
using System.Reflection;
using MoreLinq;
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
			
			foreach (var batchedInfo in streamInfos.Batch(BatchSize))
			{
				var downloadTasks = batchedInfo.Select(t => DownloadVideo(t.streamInfo, t.video));
				await Task.WhenAll(downloadTasks);
			}

			var videoNames = streamInfos.Select(t => t.video).ToList();
			
			
			var convertedName = videoNames.First()
				.Replace(" ", string.Empty)
				.Replace("!", string.Empty);
			
			var videoFile = $"tempVideos/{convertedName}.mp4";
			var audioFile = $"audios/{convertedName}.mp3";

			var snippets = CreateSnippets();
			FFmpeg.SetExecutablesPath("ffmpeg/");
			var nn = await snippets.ExtractAudio(videoFile, audioFile);
			await nn.Start();
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
			
			await using var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo);
			await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, videoFile);
		}

		private static async Task RunProcess(string videoName)
		{
			var process = CreateProcess(videoName);
			process.Start();
			var info = await process.StandardOutput.ReadToEndAsync();
			Console.WriteLine(info);
			var errors = await process.StandardError.ReadToEndAsync();
			Console.WriteLine(errors);
			await process.WaitForExitAsync();
			
			if (!process.HasExited)
				process.Kill();
		}

		private static Process CreateProcess(string videoName)
		{
			var convertedName = videoName
				.Replace(" ", string.Empty)
				.Replace("!", string.Empty);
			
			var videoFile = $"tempVideos/{convertedName}.mp4";
			var audioFile = $"audios/{convertedName}.mp3";

			return new ()
			{
				StartInfo = new ()
				{
					UseShellExecute = false,
					RedirectStandardInput = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					FileName = "ffmpeg/ffmpeg.exe",
					Arguments = $" -i {videoFile} -vn -f mp3 -ab 320k output {audioFile}"
				}
			};
		}

		private static Snippets CreateSnippets()
		{
			return (Snippets) typeof(Snippets).GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance,
				null, Type.EmptyTypes, null)!.Invoke(null);
		}
	}
}