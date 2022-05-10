using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PelotonDadsChallenge
{
    public static class PelotonDadsChallengeHttpTrigger
    {
        [FunctionName("PelotonDadsChallengeHttpTrigger")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("results")]ICollector<string> collector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            collector.Add("Collect Results");

            string responseMessage = "You have successfully requested Peloton Dads Challenge Results. Results will be emailed to you when available!";

            return new OkObjectResult(responseMessage);
        }
    }
}
