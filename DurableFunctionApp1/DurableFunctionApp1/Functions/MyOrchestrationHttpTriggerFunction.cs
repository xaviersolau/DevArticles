using DurableFunctionApp1.Business;
using DurableLib;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DurableFunctionApp1.Functions
{
    public class MyOrchestrationHttpTriggerFunction
    {
        private readonly IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory;

        public MyOrchestrationHttpTriggerFunction(IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory)
        {
            this.myOrchestrationFactory = myOrchestrationFactory;
        }

        [Function("DurableFunction_HttpStart")]
        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext, string parameter)
        {
            ILogger logger = executionContext.GetLogger("DurableFunction_HttpStart");

            var instanceId = await myOrchestrationFactory
                .NewOrchestrationAsync(client, o => o.RunOrchestrator(parameter));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
