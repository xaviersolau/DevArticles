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
        private readonly IServiceProvider serviceProvider;

        public MyOrchestrationFunction(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [Function(nameof(IMyOrchestration.RunOrchestrator))]
        public Task<List<string>> RunOrchestratorFunction(
            [OrchestrationTrigger] TaskOrchestrationContext context, string parameter)
        {
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
    }
}
