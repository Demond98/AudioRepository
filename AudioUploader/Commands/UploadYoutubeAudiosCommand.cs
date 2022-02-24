using MediatR;

namespace AudioUploader.Commands
{
	public class UploadYoutubeAudiosCommand: IRequest
	{
		public IReadOnlyCollection<string> AudioCode { get; set; }

		public UploadYoutubeAudiosCommand()
		{
			AudioCode = new List<string>();
		}
	}

	public class AudiosToUploadCommandHandler : IRequestHandler<UploadYoutubeAudiosCommand>
	{
		public async Task<Unit> Handle(UploadYoutubeAudiosCommand command, CancellationToken cancellationToken)
		{
			//TODO:
			return await Task.FromResult(new Unit());
		}
	}
}
