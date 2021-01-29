using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
	public class Function2
	{
		[FunctionName("Function2")]
		public void Run([ServiceBusTrigger("%busqueue%", Connection = "busconnectionstring")] string mySbMsg, ILogger log)
		{
			log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
		}
	}
}
