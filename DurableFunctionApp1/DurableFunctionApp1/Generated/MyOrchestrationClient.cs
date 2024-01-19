using DurableFunctionApp1.Business;
using DurableLib;

namespace DurableFunctionApp1.Generated
{
    /// <summary>
    /// Generated
    /// </summary>
    public class MyOrchestrationClient : OrchestrationClientBase, IMyOrchestration
    {
        public async Task<List<string>> RunOrchestrator(string parameter)
        {
            var instanceId = await Client.ScheduleNewOrchestrationInstanceAsync(
                nameof(IMyOrchestration.RunOrchestrator), parameter);

            Id = instanceId;

            return default!;
        }
    }
}
