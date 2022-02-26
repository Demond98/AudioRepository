using AudioUploader.Gateways;
using AudioUploader.Gateways.Interfaces;
using AudioUploader.Options;

namespace AudioUploader
{
	public static class ServiceCollectionExtensions
	{
		public static void InitializeAssets(this WebApplication webApplication)
		{
			var sp = webApplication.Services.CreateScope();
			var minIOGateway = sp.ServiceProvider.GetService<IMinIOGateway>();
			minIOGateway!.CreateBucketIfNotExist().Wait();
		}

		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.ConfigureFromSection<MinIOClientOption>(configuration, "MinIOClient");
			services.AddTransient<IMinIOGateway, MinIOGateway>();

			services.AddTransient<IYouTubeGateway, YouTubeGateway>();

			return services;
		}

		private static IServiceCollection ConfigureFromSection<T>(this IServiceCollection services, IConfiguration configuration, string path)
			where T : class
		{
			services.Configure<T>(configuration.GetSection(path));

			return services;
		}
	}
}
