using AudioUploader.Models.Entities;
using YoutubeExplode.Videos.Streams;

namespace AudioUploader.Gateways.Interfaces
{
	public interface IYouTubeGateway
	{
		Task<YoutubeVideoInfo> GetYoutubeVideoInfo(string videoCode);
		Task<Stream> GetAudioStream(IStreamInfo streamInfo);
	}
}
