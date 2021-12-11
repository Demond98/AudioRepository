using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioDbWebServer.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AudioController : ControllerBase
	{
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return id.ToString();
		}
	}
}
