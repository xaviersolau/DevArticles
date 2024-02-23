
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableLib.Isolated
{
    public class OrchestrationContextIsolated : IOrchestrationContext
    {
        private TaskOrchestrationContext context;

        public OrchestrationContextIsolated(TaskOrchestrationContext context)
        {
            this.context = context;
        }

        public Task<TResult> CallActivityAsync<TResult, TPayload>(string name, TPayload payload)
        {
            return this.context.CallActivityAsync<TResult>(name, payload);
        }

        public ILogger CreateReplaySafeLogger()
        {
            return this.context.CreateReplaySafeLogger(this.context.Name);
        }

        public ILogger CreateReplaySafeLogger(Type type)
        {
            return this.context.CreateReplaySafeLogger(type);
        }
    }
}