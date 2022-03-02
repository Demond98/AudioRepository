using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
			var title = NormilizeTitle(youtubeVideoInfo.Title);
			var extention = youtubeVideoInfo.StreamInfo.Container.Name;
			Console.WriteLine($"Yotube stream info extracted - Code = {youtubeVideoInfo.Code}, Name = {title}");

			using var stream = await _youTubeGateway.GetVideoStream(youtubeVideoInfo.StreamInfo);
			Console.WriteLine($"Youtube stream extracted - Code = {youtubeVideoInfo.Code}, Name = {title}");

			using var fileStream = File.Create(Path.Combine(VideosPath, $"{title}.{extention}"));
			await stream.CopyToAsync(fileStream);

			Console.WriteLine($"Data uploaded - Code = {youtubeVideoInfo.Code}, Name = {title}");
		}

		private static string NormilizeTitle(string title) => new Regex("[\\/:?]").Replace(title, "_");
	}
}