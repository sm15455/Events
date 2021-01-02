using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace TestApp
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
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
			Assert.NotNull(config["MyConfigurationKey"]);
		}
	}
}
