using System;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
	public class Function2
	{
		[FunctionName("Function2")]
		public void Run([ServiceBusTrigger("myqueue", Connection = "tbusconnectionstring")] string mySbMsg, ILogger log)
		{
			log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
		}
	}
}
