
using Microsoft.DurableTask.Client;

namespace DurableLib.Isolated
{
    internal class OrchestrationClientIsolated : IOrchestrationClient
    {
        private DurableTaskClient client;

        public OrchestrationClientIsolated(DurableTaskClient client)
        {
            this.client = client;
        }

        public Task<string> ScheduleNewOrchestrationInstanceAsync<TInput>(string name, TInput input) where TInput : class
        {
            return this.client.ScheduleNewOrchestrationInstanceAsync(name, input);
        }
    }
}