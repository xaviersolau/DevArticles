using DurableLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DurableLib.Simple
{
    public class OrchestrationContextSimple : IOrchestrationContext, IOrchestrationTools
    {
        private readonly IServiceProvider serviceProvider;

        public OrchestrationContextSimple(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<TResult> CallActivityAsync<TResult, TPayload>(string name, TPayload payload)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> CallSubOrchestrationAsync<TResult, TPayload>(string name, TPayload payload)
        {
            throw new NotImplementedException();
        }

        public ILogger CreateReplaySafeLogger()
        {
            return this.serviceProvider.GetRequiredService<ILogger>();
        }

        public ILogger CreateReplaySafeLogger(Type type)
        {
            return this.serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(type);
        }

        public IOrchestrationTools GetOrchestrationTools()
        {
            return this;
        }

        public Task SendEvent<T>(string id, string eventName, T eventToSend) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default) where T : IEvent
        {
            throw new NotImplementedException();
        }
    }
}
