using AudioUploader.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AudioUploader.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class YoutubeAudioController : ControllerBase
	{
		private readonly IMediator _mediator;

		public YoutubeAudioController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost(Name = "UploadAudios")]
		public async Task<OkResult> UploadAudios([FromBody] UploadYoutubeAudiosCommand command)
		{
			/*
			 {
			  "audioCodes": [
				"1-emQo-7O3Y",
				"jWSnW89PAS4"
			  ]
			}*/

			_ = await _mediator.Send(command);

			return Ok();
		}
	}
}
