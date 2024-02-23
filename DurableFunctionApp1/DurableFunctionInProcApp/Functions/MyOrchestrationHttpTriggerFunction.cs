using DurableFunctionApp1.Business;
using DurableLib;
using DurableLib.InProc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunctionInProcApp.Functions
{
    public class MyOrchestrationHttpTriggerFunction
    {
        private readonly IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory;

        public MyOrchestrationHttpTriggerFunction(IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory)
        {
            this.myOrchestrationFactory = myOrchestrationFactory;
        }

        [FunctionName("DurableFunction_HttpStart")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger logger)
        {
            var parameter = req.Query["parameter"];

            var instanceId = await myOrchestrationFactory
                .NewOrchestrationAsync(client, o => o.RunOrchestrator(parameter));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
