using AudioUploader.Models.Entities;
using YoutubeExplode.Videos.Streams;

namespace AudioUploader.Gateways.Interfaces
{
	public interface IYouTubeGateway
	{
		Task<YoutubeVideoInfo> GetYoutubeVideoInfos(IEnumerable<string> videoCodes);
		Task<Stream> GetAudioStream(IStreamInfo streamInfo);
	}
}
