using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioUploaderIntegrationTests
{
	public static class Extensions
	{
		public static async Task<TResponse> SendRequestByMediator<TRequest, TResponse>(this IntegrationTestWebApplicationFactory factory, TRequest request)
		{
			using var sp = factory.Server.Services.CreateScope();
			var mediator = sp.ServiceProvider.GetService<IMediator>()!;

			var response = await mediator.Send(request!, default);

			return (TResponse) response!;
		}

		public static async Task SendRequestByMediator<TRequest>(this IntegrationTestWebApplicationFactory factory, TRequest request)
		{
			using var sp = factory.Server.Services.CreateScope();
			var mediator = sp.ServiceProvider.GetService<IMediator>()!;

			_ = await mediator.Send(request!, default);
		}
	}
}
