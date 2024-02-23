using DurableTask.Core;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableLib.InProc
{
    /// <summary>
    /// From Lib
    /// </summary>
    public static class OrchestrationFactoryExtensions
    {
        public static Task<string> NewOrchestrationAsync<TOrchestration>(
            this IOrchestrationFactory<TOrchestration> orchestrationFactory,
            IDurableOrchestrationClient client,
            Func<TOrchestration, Task> action)
        {
            var orchestrationClient = new OrchestrationClientInProc(client);
            return orchestrationFactory.NewOrchestrationAsync(orchestrationClient, action);
        }
    }
}
