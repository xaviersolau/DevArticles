using DurableFunctionApp1.Business;
using DurableLib;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.DependencyInjection;

namespace DurableFunctionApp1.Generated
{
    /// <summary>
    /// Generated
    /// </summary>
    public class MyOrchestrationFunction
    {
        public record RunOrchestratorPayload
        {
            public string Parameter { get; set; }
        }

        private readonly IServiceProvider serviceProvider;

        public MyOrchestrationFunction(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [Function(nameof(IMyOrchestration.RunOrchestrator))]
        public Task<List<string>> RunOrchestratorFunction(
            [OrchestrationTrigger] TaskOrchestrationContext context, RunOrchestratorPayload payload)
        {
            var parameter = payload.Parameter;

            var orchestrationCtx = this.serviceProvider.GetRequiredService<OrchestrationCtx>();

            orchestrationCtx.SetContext(context);
            try
            {
                var myOrchestration = this.serviceProvider.GetRequiredService<IMyOrchestration>();

                return myOrchestration.RunOrchestrator(parameter);
            }
            finally
            {
                orchestrationCtx.SetContext(null);
            }
        }

        /// <summary>
        /// Generated
        /// </summary>
        public sealed class Client : OrchestrationClientBase, IMyOrchestration
        {
            public async Task<List<string>> RunOrchestrator(string parameter)
            {
                var payload = new MyOrchestrationFunction.RunOrchestratorPayload
                {
                    Parameter = parameter,
                };

                var instanceId = await Client.ScheduleNewOrchestrationInstanceAsync(
                    nameof(IMyOrchestration.RunOrchestrator), payload);

                Id = instanceId;

                return default!;
            }
        }

        public sealed class ClientFactory : OrchestrationFactory<IMyOrchestration, Client>
        {
        }
    }
}
