using YoutubeExplode.Videos.Streams;

namespace AudioUploader.Models.Entities
{
	public class YoutubeVideoInfo
	{
		public string Code { get; init; }
		public IStreamInfo StreamInfo { get; init; }
	}
}
