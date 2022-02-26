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
			await tasks.Execute(BatchSize);
		}

		private static async Task UploadAudio(UploadYoutubeAudiosFunctor functor, string audioCode)
		{
			var youtubeVideoInfo = await functor.GetYoutubeVideoInfo(audioCode);
			functor.LogInformation($"Yotube stream info extracted - Code = {youtubeVideoInfo.Code}");

			using var stream = await functor.GetAudioStream(youtubeVideoInfo.StreamInfo);
			functor.LogInformation($"Youtube stream extracted - Code = {youtubeVideoInfo.Code}");

			await functor.UploadAudio(stream, audioCode);
			functor.LogInformation($"Audio uploaded to file storage - Code = {youtubeVideoInfo.Code}");
		}
	}
}
