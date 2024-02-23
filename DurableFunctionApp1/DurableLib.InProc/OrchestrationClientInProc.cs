

using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableLib.InProc
{
    public class OrchestrationClientInProc : IOrchestrationClient
    {
        private IDurableOrchestrationClient client;

        public OrchestrationClientInProc(IDurableOrchestrationClient client)
        {
            this.client = client;
        }

        public Task<string> ScheduleNewOrchestrationInstanceAsync<TInput>(string name, TInput input)
            where TInput : class
        {
            return this.client.StartNewAsync<TInput>(name, input);
        }
    }
}