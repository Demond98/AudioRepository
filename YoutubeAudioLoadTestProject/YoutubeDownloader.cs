using System.IO.Compression;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YoutubeAudioLoadTestProject
{
	public static class YoutubeDownloader
	{
		public static async Task Load(string playListAddress)
		{
			var youtube = new YoutubeClient();
			var videos = await youtube.Playlists.GetVideosAsync(playListAddress);

			var streamInfoTasks = videos.Select(async v => await GetStreamInfo(youtube, v)).ToList();
			await Task.WhenAll(streamInfoTasks);

			var downloadTasks = streamInfoTasks.Select(async t => await DownloadVideo(youtube, t.Result.streamInfo, t.Result.video)).ToList();
			await Task.WhenAll(downloadTasks);
		}

		private static async Task<(IStreamInfo streamInfo, string video)> GetStreamInfo(YoutubeClient youtube, IVideo video)
		{
			var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

			var streamInfo = streamManifest.GetAudioStreams().MinBy(z => z.Size);

			return (streamInfo, video.Title);
		}

		private static async Task DownloadVideo(YoutubeClient youtube, IStreamInfo streamInfo, string videoName)
		{
			var convertedName = videoName
				.Replace(" ", string.Empty)
				.Replace("!", string.Empty);

			var videoFile = $"tempVideos/{convertedName}.mp3";

			using var audioStream = await youtube.Videos.Streams.GetAsync(streamInfo);
			using var targetStream = File.Create(videoFile);

			await audioStream.CopyToAsync(targetStream);
		}
	}
}
