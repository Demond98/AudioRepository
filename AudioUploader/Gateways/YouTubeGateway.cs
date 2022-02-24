using AudioUploader.Gateways.Interfaces;
using AudioUploader.Models.Entities;
using YoutubeExplode.Videos.Streams;

namespace AudioUploader.Gateways
{
	public class YouTubeGateway : IYouTubeGateway
	{
		public Task<Stream> GetAudioStream(IStreamInfo streamInfo)
		{
			throw new NotImplementedException();
		}

		public Task<YoutubeVideoInfo> GetYoutubeVideoInfos(IEnumerable<string> videoCodes)
		{
			throw new NotImplementedException();
		}
	}
}
