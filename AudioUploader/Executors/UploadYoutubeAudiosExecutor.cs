using AudioUploader._Common.Extensions;
using AudioUploader.Commands;
using AudioUploader.Functors;

namespace AudioUploader.Executors
{
	public static class UploadYoutubeAudiosExecutor
	{
		private const int BatchSize = 5;

		public static async Task Execute(UploadYoutubeAudiosCommand command, UploadYoutubeAudiosFunctor functor)
		{
			var tasks = command.AudioCodes.Select(async z => await UploadAudio(functor, z));
			await BatchManager.Execute(tasks, BatchSize);
		}

		private static async Task UploadAudio(UploadYoutubeAudiosFunctor functor, string audioCode)
		{
			var youtubeVideoInfo = await functor.GetYoutubeVideoInfo(audioCode);
			using var stream = await functor.GetAudioStream(youtubeVideoInfo.StreamInfo);
			await functor.UploadAudio(stream, audioCode);
		}
	}
}
