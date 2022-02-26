namespace AudioUploader.Gateways.Interfaces
{
	public interface IMinIOGateway
	{
		Task CreateBucketIfNotExist();
		Task UploadAudio(Stream audioStream, string videoCode);
	}
}
