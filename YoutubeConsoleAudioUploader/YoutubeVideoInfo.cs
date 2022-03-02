using YoutubeExplode.Videos.Streams;

namespace YoutubeConsoleUploader
{
	public class YoutubeVideoInfo
	{
		public string Code { get; init; }
		public IStreamInfo StreamInfo { get; init; }
	}
}