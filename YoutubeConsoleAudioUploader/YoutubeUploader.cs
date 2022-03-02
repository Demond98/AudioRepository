using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeConsoleUploader
{
	public class YoutubeUploader
	{
		public const string VideosPath = "Videos";
		private readonly Setting _setting;
		private readonly YouTubeGateway _youTubeGateway;

		public YoutubeUploader(Setting setting)
		{
			_setting = setting;
			_youTubeGateway = new YouTubeGateway();
		}

		private string Extension => _setting.IsAudio ? "mp3" : "mp4";

		public async Task Upload()
		{
			Console.WriteLine("Upload started");
			var tasks = _setting.YoutubeLinks.Select(async z => await UploadAudio(z));
			await tasks.Execute(_setting.BatchSize);
		}

		private async Task UploadAudio(string code)
		{
			var youtubeVideoInfo = await _youTubeGateway.GetYoutubeVideoInfo(code, _setting.IsAudio);
			Console.WriteLine($"Yotube stream info extracted - Code = {youtubeVideoInfo.Code}");

			using var stream = await _youTubeGateway.GetVideoStream(youtubeVideoInfo.StreamInfo);
			Console.WriteLine($"Youtube stream extracted - Code = {youtubeVideoInfo.Code}");

			using var fileStream = File.Create($"{VideosPath}/{code}.{Extension}");
			stream.Seek(0, SeekOrigin.Begin);
			await stream.CopyToAsync(fileStream);
			fileStream.Close();

			Console.WriteLine($"Data uploaded - Code = {youtubeVideoInfo.Code}");
		}
	}
}
