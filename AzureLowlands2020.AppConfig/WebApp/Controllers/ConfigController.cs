using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ConfigController
	{
		IConfiguration _configuration;
		IOptionsSnapshot<ConfigRoot> _configRoot;

		public ConfigController(IConfiguration configuration, IOptionsSnapshot<ConfigRoot> configRoot)
		{
			_configuration = configuration;
			_configRoot = configRoot;
		}

		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new[] {
				_configuration.GetValue<string>("Background"),
				_configRoot.Value.Background,
				_configRoot.Value.SMTP.Server
			};
		}
	}
}
