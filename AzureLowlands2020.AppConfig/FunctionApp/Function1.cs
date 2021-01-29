using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
	public class Function1
	{
		readonly IConfiguration _configuration;
		IOptionsSnapshot<ConfigRoot> _optionValue;
		IConfigurationRefresherProvider _configurationRefresherProvider;

		public Function1(IConfiguration configuration, IOptionsSnapshot<ConfigRoot> optionValue, IConfigurationRefresherProvider configurationRefresherProvider)
		{
			_configuration = configuration;
			_optionValue = optionValue;
			_configurationRefresherProvider = configurationRefresherProvider;
		}

		[FunctionName("Function1")]
		public IEnumerable<string> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
		{
			_ = _configurationRefresherProvider.Refreshers.First().TryRefreshAsync();
			return new[]
			{
				_configuration["Background"],
				_optionValue.Value.Background,
				_optionValue.Value.SMTP.Server,
				_optionValue.Value.SMTPJson.Server
			};
		}
	}
}
