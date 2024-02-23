using Microsoft.DurableTask.Client;

namespace DurableLib.Isolated
{
    /// <summary>
    /// From Lib
    /// </summary>
    public static class OrchestrationFactoryExtensions
    {
        public static Task<string> NewOrchestrationAsync<TOrchestration>(
            this IOrchestrationFactory<TOrchestration> orchestrationFactory,
            DurableTaskClient client,
            Func<TOrchestration, Task> action)
        {
            var orchestrationClient = new OrchestrationClientIsolated(client);
            return orchestrationFactory.NewOrchestrationAsync(orchestrationClient, action);
        }
    }
}
