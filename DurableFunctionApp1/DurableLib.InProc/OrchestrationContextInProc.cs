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

        public Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default) where T : IEvent
        {
            return this.context.WaitForExternalEvent<T>(eventName, TimeSpan.FromMinutes(5), cancellationToken);
        }

        public Task SendEvent<T>(string id, string eventName, T eventToSend) where T : IEvent
        {
            var payload = new SendEventPayload<T>()
            {
                InstanceId = id,
                EventName = eventToSend.GetType().Name,
                Event = eventToSend
            };

            return this.context.CallActivityAsync("SendEventInternal" + typeof(T).Name, payload);
        }
    }
    public class SendEventPayload<T> where T : IEvent
    {
        public string InstanceId { get; set; }

        public string EventName { get; set; }

        public T Event { get; set; }
    }
}