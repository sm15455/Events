using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var settings = config.Build();
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

                            config.AddAzureAppConfiguration(options =>
                                options
                                    .Connect(new Uri("https://cfg-appconfig-demo.azconfig.io"), credentials)
                                    .ConfigureRefresh(c =>
                                    {
                                        c
                                            .Register("Sentinel", true)
                                            .SetCacheExpiration(new TimeSpan(0, 0, 660));
                                    })
                                    .ConfigureKeyVault(kv =>
                                    {
                                        kv.SetCredential(credentials);
                                    })
                            );
                        })

                        .UseStartup<Startup>();
                });
    }
}
