using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAudioLoadTestProject
{
	internal static class FileUploader
	{
		public static async Task UploadFile(string path)
		{
			const string connection = "BlobEndpoint=localhost:443";
			const string audioContainerName = "audios";

			var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
			var container = blobServiceClient.GetBlobContainerClient(audioContainerName);
			await container.CreateIfNotExistsAsync();

			using var ms = new MemoryStream();

			await File.OpenRead(path).CopyToAsync(ms);
			ms.Position = 0;

			await container.UploadBlobAsync(path, ms);

			var blobClient = container.GetBlobClient(path);

			using var newMs = new MemoryStream();
			await blobClient.DownloadToAsync(newMs);

			if (!Directory.Exists("kek"))
				Directory.CreateDirectory("kek");

			await File.WriteAllBytesAsync("kek/kek.mp3", ms.ToArray());
		}
	}
}
