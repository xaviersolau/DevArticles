using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableLib.InProc
{
    public class OrchestrationContextInProc : IOrchestrationContext
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
    }
}