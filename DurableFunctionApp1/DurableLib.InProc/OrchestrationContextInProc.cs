using DurableLib.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableLib.InProc
{
    public class OrchestrationContextInProc : IOrchestrationContext, IOrchestrationTools
    {
        private IDurableOrchestrationContext context;
        private readonly ILogger logger;

        public OrchestrationContextInProc(IDurableOrchestrationContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public Task<TResult> CallActivityAsync<TResult, TPayload>(string name, TPayload payload)
        {
            return this.context.CallActivityAsync<TResult>(name, payload);
        }

        public ILogger CreateReplaySafeLogger()
        {
            return this.context.CreateReplaySafeLogger(this.logger);
        }

        public ILogger CreateReplaySafeLogger(Type type)
        {
            return this.context.CreateReplaySafeLogger(this.logger);
        }

        public IOrchestrationTools GetOrchestrationTools()
        {
            return this;
        }

        public Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default)
        {
            return this.context.WaitForExternalEvent<T>(eventName);
        }
    }
}