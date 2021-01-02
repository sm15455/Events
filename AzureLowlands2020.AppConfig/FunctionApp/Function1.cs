using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var tmpConfig = builder.GetContext().Configuration;

            // create a new configurationbuilder and add appconfiguration
            builder.ConfigurationBuilder.AddAzureAppConfiguration((options) =>
            {
                var credentials = new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        ExcludeEnvironmentCredential = true,
                        ExcludeInteractiveBrowserCredential = true,
                        ExcludeSharedTokenCacheCredential = true,
                        ExcludeVisualStudioCodeCredential = true,
                        ExcludeVisualStudioCredential = true
                    }
                );

                options
                    .Connect(new Uri("https://cfg-appconfig-demo.azconfig.io"), credentials)
                    .ConfigureKeyVault(kv =>
                    {
                        kv.SetCredential(credentials);
                    });

                // configure appconfiguation features you want;
                // options.UseFeatureFlags();
                // options.Select(KeyFilter.Any, LabelFilter.Null);
            });
        }
    }

    //public class Function1
    //{
    //    readonly IConfiguration _configuration;

    //    public Function1(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    [FunctionName("Function1")]
    //    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
    //    {
    //        return new OkObjectResult(_configuration["SecretKey"]);
    //    }
    //}
}
