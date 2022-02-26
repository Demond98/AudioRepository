using AudioUploader.Gateways.Interfaces;
using AudioUploader.Options;
using Microsoft.Extensions.Options;
using Minio;

namespace AudioUploader.Gateways
{
	public class MinIOGateway : IMinIOGateway
	{
		private const string ContentType = "application/mpeg";

		private readonly MinioClient _minioClient;
		public MinIOGateway(IOptions<MinIOClientOption> options)
		{
			Options = options.Value;
			_minioClient = new MinioClient(Options.Address, Options.AccessKey, Options.SecretKey);
		}

		private MinIOClientOption Options { get; }

		public async Task CreateBucketIfNotExist()
		{
			var bucketName = Options.Bucket;
			var found = await _minioClient.BucketExistsAsync(bucketName);
			if (!found)
				await _minioClient.MakeBucketAsync(bucketName, Options.Location);
		}

		public async Task UploadAudio(Stream audioStream, string videoCode)
		{
			await _minioClient.PutObjectAsync(Options.Bucket, videoCode, audioStream, audioStream.Length, ContentType);
		}
	}
}
