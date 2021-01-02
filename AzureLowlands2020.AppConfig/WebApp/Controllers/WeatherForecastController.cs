using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController
	{
		IConfiguration _configuration;
		public WeatherForecastController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private static readonly string[] Summaries = new[]
		{
						"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
				};

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("Show")]
		public string GetShow()
		{
			return _configuration.GetValue<string>("MyConfigurationKey");
		}

		[HttpGet("Hook")]
		public string GetHook()
		{
			return "ok";
		}
	}
}
