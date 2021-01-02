using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
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
							//ExcludeEnvironmentCredential = true,
							//ExcludeInteractiveBrowserCredential = true,
							ExcludeSharedTokenCacheCredential = true,
							//ExcludeVisualStudioCodeCredential = true,
							//ExcludeVisualStudioCredential = true
						}
				);

				options
						.Connect(new Uri("https://cfg-appconfig-demo.azconfig.io"), credentials)
						//.ConfigureKeyVault(kv =>
						//{
						//	kv.SetCredential(credentials);
						//})
						.Select("MyConfigurationKey")
						////.Select("MyConfigurationKey", "Dev")
						//.Select("MyCompositeKey*")
						//.Select("json")
						//.Select("asset:*").TrimKeyPrefix("asset:")
						;
			});

			var config = builder.Build();
			Console.WriteLine(config["json"]);
		}
	}
}
