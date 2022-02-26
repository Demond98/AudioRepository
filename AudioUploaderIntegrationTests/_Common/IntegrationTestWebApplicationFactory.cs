using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AudioUploaderIntegrationTests
{
	[CollectionDefinition(nameof(IntegrationTestsCollection))]
	public class IntegrationTestsCollection: ICollectionFixture<IntegrationTestWebApplicationFactory>
	{
	}

	public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
	{ 
	}

	public class IntegrationTestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
	{
	}
}
