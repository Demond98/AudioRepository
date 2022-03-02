using AudioUploader.Commands;
using AudioUploaderIntegrationTests._Common;
using FluentAssertions;
using MediatR;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace AudioUploaderIntegrationTests
{
	[Collection(nameof(IntegrationTestsCollection))]
	public class YoutubeAudiosTests
	{
		private static string RequestsPath => $"{Consts.DefaultRequestPath}/{nameof(YoutubeAudiosTests)}";

		private readonly IntegrationTestWebApplicationFactory _factory;

		public YoutubeAudiosTests(IntegrationTestWebApplicationFactory factory)
		{
			_factory = factory;
		}

		[Fact]
		public async void Check_audio_upload()
		{
			//arrange
			var requestFilePath = $"{RequestsPath}/{nameof(Check_audio_upload)}.json";
			var text = File.ReadAllText(requestFilePath);
			var command = JsonConvert.DeserializeObject<UploadYoutubeAudiosCommand>(text);

			//act
			await _factory.SendRequestByMediator(command);

			//assert
		}
	}
}