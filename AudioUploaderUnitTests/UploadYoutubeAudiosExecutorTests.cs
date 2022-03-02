using AudioUploader.Commands;
using AudioUploader.Executors;
using AudioUploader.Functors;
using AudioUploader.Models.Entities;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using YoutubeExplode.Videos.Streams;

namespace AudioUploaderUnitTests
{
	public class UploadYoutubeAudiosExecutorTests
	{
		[Fact]
		public async void Check_upload_youtube_audio_correct()
		{
			//arrange
			const int count = 20;

			var command = new UploadYoutubeAudiosCommand()
			{
				AudioCodes = GetTestAudioCodes(count)
			};

			var getYoutubeVideoInfoCallsCount = 0;
			var getAudioStreamCount = 0;
			var uploadAudioCount = 0;

			var functor = new UploadYoutubeAudiosFunctor()
			{
				GetYoutubeVideoInfo = _ => AsyncMethodStub(ref getYoutubeVideoInfoCallsCount, new YoutubeVideoInfo()),
				GetAudioStream = _ => AsyncMethodStub(ref getAudioStreamCount, Stream.Null),
				UploadAudio = (_, __) => AsyncVoidMethodStub(ref uploadAudioCount),
				LogInformation = _ => { }
			};

			//act
			await UploadYoutubeAudiosExecutor.Execute(command, functor);

			//assert
			getYoutubeVideoInfoCallsCount.Should().Be(count);
			getAudioStreamCount.Should().Be(count);
			uploadAudioCount.Should().Be(count);
		}

		private static IReadOnlyCollection<string> GetTestAudioCodes(int count)
		{
			return Enumerable.Range(0, count).Select(i => i.ToString()).ToArray();
		}

		private static Task AsyncVoidMethodStub(ref int counter)
		{
			Interlocked.Increment(ref counter);

			return Task.CompletedTask;
		}

		private static Task<T> AsyncMethodStub<T>(ref int counter, T value)
		{
			Interlocked.Increment(ref counter);

			return Task.FromResult(value);
		}
	}
}