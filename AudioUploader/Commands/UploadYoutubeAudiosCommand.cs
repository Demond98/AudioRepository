using AudioUploader.Executors;
using AudioUploader.Functors;
using AudioUploader.Gateways.Interfaces;
using MediatR;

namespace AudioUploader.Commands
{
	public class UploadYoutubeAudiosCommand: IRequest
	{
		public IReadOnlyCollection<string> AudioCodes { get; init; }
	}

	public class AudiosToUploadCommandHandler : IRequestHandler<UploadYoutubeAudiosCommand>
	{
		private readonly IMinIOGateway _minIOGateway;
		private readonly IYouTubeGateway _youTubeGateway;

		public AudiosToUploadCommandHandler(IMinIOGateway minIOGateway, IYouTubeGateway youTubeGateway)
		{
			_minIOGateway = minIOGateway;
			_youTubeGateway = youTubeGateway;
		}

		public async Task<Unit> Handle(UploadYoutubeAudiosCommand command, CancellationToken cancellationToken)
		{
			var functor = new UploadYoutubeAudiosFunctor
			{
				GetYoutubeVideoInfo = _youTubeGateway.GetYoutubeVideoInfo,
				GetAudioStream = _youTubeGateway.GetAudioStream,
				UploadAudio = _minIOGateway.UploadAudio
			};

			await UploadYoutubeAudiosExecutor.Execute(command, functor);

			return new Unit();
		}
	}
}
