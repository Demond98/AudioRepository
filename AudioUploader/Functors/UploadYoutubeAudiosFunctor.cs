using AudioUploader.Models.Entities;
using YoutubeExplode.Videos.Streams;

namespace AudioUploader.Functors
{
	public class UploadYoutubeAudiosFunctor
	{
		//Task<YoutubeVideoInfo> GetYoutubeVideoInfo(string videoCode);
		public Func<string, Task<YoutubeVideoInfo>> GetYoutubeVideoInfo { get; init; }

		//Task<Stream> GetAudioStream(IStreamInfo streamInfo);
		public Func<IStreamInfo, Task<Stream>> GetAudioStream { get; init; }

		//Task UploadAudio(Stream audioStream, string videoCode);
		public Func<Stream, string, Task> UploadAudio { get; init; }

		public Action<string> LogInformation { get; init; }
	}
}
