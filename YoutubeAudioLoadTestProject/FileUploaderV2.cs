using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAudioLoadTestProject
{
	public static class FileUploaderV2
	{
		public static async Task Upload()
		{
			const string endpoint = "localhost:9000";
			const string accessKey = "minio_access_key";
			const string secretKey = "minio_secret_key";
			const string location = "us-east-1";
			const string bucketName = "music";

			var minio = new MinioClient(endpoint, accessKey, secretKey);

			var found = await minio.BucketExistsAsync("music");
			if (!found)
				await minio.MakeBucketAsync("music", location);

			await minio.PutObjectAsync(bucketName, "BringIt.mp3", "tempVideos/BringIt.mp3", "application/mpeg");

			Directory.CreateDirectory("kek");
			await minio.GetObjectAsync(bucketName, "BringIt.mp3", 
				stream => 
				{
					using var fileStream = File.Create("kek/keknul.mp3");
					stream.CopyTo(fileStream);
				});
		}
	}
}
