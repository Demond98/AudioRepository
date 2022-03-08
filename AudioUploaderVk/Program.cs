using Minio;
using System.Net;
using VkNet;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPut("/upload/{ownerId}", async (long ownerId) =>
{
	var vk = new VkApi();
	var minioClient = new MinioClient()
		.WithEndpoint(builder.Configuration["MINIO_ENDPOINT"])
		.WithCredentials(builder.Configuration["MINIO_ACCESS_KEY"], builder.Configuration["MINIO_SECRET_KEY"])
		.Build();

	await vk.AuthorizeAsync(new VkNet.Model.ApiAuthParams()
	{
		AccessToken = builder.Configuration["VK_API_TOKEN"],
	});

	var audios = await vk.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
	{
		OwnerId = ownerId,
		Count = 6000,
	});

	var audiosIds = audios.Select(a => $"{ownerId}_{a.Id}");
	var audiosInfo = await vk.Audio.GetByIdAsync(audiosIds);
	
	using var client = new HttpClient();
	foreach (var audio in audiosInfo)
	{
		try
		{
			var bytes = await client.GetByteArrayAsync(audio.Url);
			var memory = new MemoryStream(bytes);

			await minioClient.PutObjectAsync(new PutObjectArgs()
				.WithBucket(builder.Configuration["MINIO_BUCKET"])
				.WithObject($"{audio.OwnerId}_{audio.Id}.mp3")
				.WithStreamData(memory)
				.WithObjectSize(memory.Length)
				.WithContentType("application/mp3"));
		}
		catch
		{
			app.Logger.LogError($"File not loaded: {audio.OwnerId}_{audio.Id}.mp3, '{audio.Title}'");
		}
	}
})
.WithName("Upload");

app.Run();