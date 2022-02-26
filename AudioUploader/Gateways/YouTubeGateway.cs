using AudioUploader.Gateways.Interfaces;
using AudioUploader.Models.Entities;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace AudioUploader.Gateways
{
	public class YouTubeGateway : IYouTubeGateway
	{
		private readonly YoutubeClient _youtubeClient;

		public YouTubeGateway()
		{
			_youtubeClient = new YoutubeClient();
		}

		private StreamClient StreamClient => _youtubeClient.Videos.Streams;

		public async Task<Stream> GetAudioStream(IStreamInfo streamInfo) => await StreamClient.GetAsync(streamInfo);

		public async Task<YoutubeVideoInfo> GetYoutubeVideoInfo(string videoCode)
		{
			var streamManifest = await StreamClient.GetManifestAsync(videoCode);

			var streamInfo = streamManifest
				.GetAudioOnlyStreams()
				.Where(z => z.Container == Container.WebM)
				.GetWithHighestBitrate();

			return new()
			{
				Code = videoCode,
				StreamInfo = streamInfo
			};
		}
	}
}
