namespace AudioUploader.Gateways.Interfaces
{
	public interface IMinIOGateway
	{
		Task CreateBucketIfNotExist(string bucketName);
		Task UploadAudio(Stream audioStream, string videoCode);
	}
}
