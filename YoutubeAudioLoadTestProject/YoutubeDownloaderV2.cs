using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace YoutubeAudioLoadTestProject
{
	public static class YoutubeDownloaderV2
	{
		public static async Task Load(string url)
		{
			var youtube = new YoutubeClient();
			var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);

			var streamInfo = streamManifest.GetAudioStreams().MinBy(z => z.Size);

			Directory.CreateDirectory("lol");
			await youtube.Videos.Streams.DownloadAsync(streamInfo!, "lol/lol.mp3");
		}
	}
}
