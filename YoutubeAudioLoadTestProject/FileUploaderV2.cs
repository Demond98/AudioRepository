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
			const string endpoint = "http://127.0.0.1:9000";
			const string accessKey = "minio_access_key";
			const string secretKey = "minio_secret_key";
			const string location = "us-east-1";
			const string bucketName = "mymusic";

			var minio = new MinioClient(endpoint, accessKey, secretKey);

			var found = await minio.BucketExistsAsync("music");
			if (!found)
				await minio.MakeBucketAsync("music", location);

			await minio.PutObjectAsync(bucketName, "kek", "tempVideos/BringIt.mp3", "mp3");
			await minio.GetObjectAsync(bucketName, "kek", "lol.mp3");
		}
	}
}
