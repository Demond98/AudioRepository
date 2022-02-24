using AudioUploader.Gateways.Interfaces;

namespace AudioUploader.Gateways
{
	public class MinIOGateway : IMinIOGateway
	{
		public Task CreateBucketIfNotExist(string bucketName)
		{
			throw new NotImplementedException();
		}

		public Task UploadAudio(Stream audioStream, string videoCode)
		{
			throw new NotImplementedException();
		}
	}
}
