using Minio;
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
			Console.WriteLine("Получен плейлист");

			var streamInfoTasks = videos.Select(async v => await GetStreamInfo(youtube, v)).ToList();
			await Task.WhenAll(streamInfoTasks);
			Console.WriteLine("Получена информация об аудио");

			var downloadTasks = streamInfoTasks.Select(async t => await DownloadVideo(youtube, t.Result.streamInfo, t.Result.video)).ToList();
			await Task.WhenAll(downloadTasks);

			Console.WriteLine("Аудио выгружены");
		}

		private static async Task<(IStreamInfo streamInfo, string video)> GetStreamInfo(YoutubeClient youtube, IVideo video)
		{
			var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

			var streamInfo = streamManifest
				.GetAudioOnlyStreams()
				.Where(z => z.Container == Container.WebM)
				.GetWithHighestBitrate();

			return (streamInfo, video.Title);
		}

		private static async Task DownloadVideo(YoutubeClient youtube, IStreamInfo streamInfo, string videoName)
		{
			const string endpoint = "localhost:9000";
			const string accessKey = "minio_access_key";
			const string secretKey = "minio_secret_key";
			const string location = "us-east-1";
			const string bucketName = "music";
			const string contentType = "application/mpeg";

			var convertedName = videoName
				.Replace(" ", string.Empty)
				.Replace("!", string.Empty);

			var videoFileName = $"{convertedName}.mp3";
			var videoFilePath = $"tempVideos/{videoFileName}";

			var minio = new MinioClient(endpoint, accessKey, secretKey);

			var found = await minio.BucketExistsAsync(bucketName);
			if (!found)
				await minio.MakeBucketAsync(bucketName, location);

			using var audioStream = await youtube.Videos.Streams.GetAsync(streamInfo);

			await minio.PutObjectAsync(bucketName, videoFileName, audioStream, audioStream.Length, contentType);

			/* Загрузка из MinIO
			Directory.CreateDirectory("kek");

			await minio.GetObjectAsync(bucketName, videoFileName,
				stream =>
				{
					using var fileStream = File.Create($"kek/{videoFileName}");
					stream.CopyTo(fileStream);
				});
			*/
		}
	}
}
