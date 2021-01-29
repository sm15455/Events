using Azure.Identity;
using ClassLibrary;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder
				.Services
				.AddOptions<ConfigRoot>()
				.Configure<IConfiguration>((cr, config) => config.Bind(cr));
			builder.Services.AddAzureAppConfiguration();
		}

		public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
		{
			// create a new configurationbuilder and add appconfiguration
			builder.ConfigurationBuilder.AddAzureAppConfiguration((options) =>
			{
				var credentials = new DefaultAzureCredential(
					new DefaultAzureCredentialOptions
					{
						ExcludeSharedTokenCacheCredential = true,
					});

				options
					.Connect(new Uri("https://cfg-appconfig-demo.azconfig.io"), credentials)
					.ConfigureRefresh(c =>
					{
						c
							.Register("Sentinel", true)
							.SetCacheExpiration(new TimeSpan(0, 0, 10));
					});
			});
		}
	}
}
