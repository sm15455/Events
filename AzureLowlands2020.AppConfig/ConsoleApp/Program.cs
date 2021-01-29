using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder();
			builder.AddAzureAppConfiguration(options =>
			{
				var credentials = new DefaultAzureCredential(
						new DefaultAzureCredentialOptions
						{
							ExcludeSharedTokenCacheCredential = true,
						}
				);

				options
						.Connect(new Uri("https://cfg-appconfig-demo.azconfig.io"), credentials)
						;
			});
			var config = builder.Build();
			Console.WriteLine(config["Background"]);


			//var builder = new ConfigurationBuilder();
			//builder.AddAzureAppConfiguration(options =>
			//{
			//	var credentials = new DefaultAzureCredential(
			//			new DefaultAzureCredentialOptions
			//			{
			//				ExcludeSharedTokenCacheCredential = true,
			//			}
			//	);

			//	options
			//			.Connect(new Uri("https://cfg-appconfig-demo.azconfig.io"), credentials)
			//			//.Select("OverrideKey")
			//			.Select("*")
			//			.Select("Default:*")
			//			.Select("ConsoleApp:*")
			//			.TrimKeyPrefix("Default:")
			//			.TrimKeyPrefix("ConsoleApp:");
			//});
			//var config = builder.Build();
			//Console.WriteLine(config["OverrideKey"]);
			//Console.WriteLine(String.Join(",", config.GetSection("Json").GetChildren().Select(c => c.Value)));
		}
	}
}
