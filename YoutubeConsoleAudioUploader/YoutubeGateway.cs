using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeConsoleUploader
{
	public class YouTubeGateway
	{
		private readonly YoutubeClient _youtubeClient;

		public YouTubeGateway()
		{
			_youtubeClient = new YoutubeClient();
		}

		private StreamClient StreamClient => _youtubeClient.Videos.Streams;

		public async Task<Stream> GetVideoStream(IStreamInfo streamInfo) => await StreamClient.GetAsync(streamInfo);

		public async Task<YoutubeVideoInfo> GetYoutubeVideoInfo(string videoCode, bool isAudio = false)
		{
			var video = await _youtubeClient.Videos.GetAsync(videoCode);
			var streamManifest = await StreamClient.GetManifestAsync(videoCode);

			var streamInfo = isAudio
				? GetAudioStreamInfo(streamManifest)
				: GetVideoStreamInfo(streamManifest);

			return new()
			{
				Code = videoCode,
				Title = video.Title,
				StreamInfo = streamInfo
			};
		}

		private static IStreamInfo GetAudioStreamInfo(StreamManifest streamManifest)
		{
			return streamManifest
				.GetAudioOnlyStreams()
				.Where(z => z.Container == Container.WebM)
				.GetWithHighestBitrate();
		}

		private static IStreamInfo GetVideoStreamInfo(StreamManifest streamManifest)
		{
			return streamManifest
				.GetMuxedStreams()
				.GetWithHighestBitrate();
		}
	}
}
