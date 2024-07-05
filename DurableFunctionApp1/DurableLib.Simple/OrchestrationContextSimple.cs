using DurableLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace DurableLib.Simple
{
    public class OrchestrationContextSimple : IOrchestrationContext, IOrchestrationTools
    {
        private readonly IServiceProvider serviceProvider;

        private Dictionary<string, Channel<IEvent>> eventQueues = new Dictionary<string, Channel<IEvent>>();

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
            return this.serviceProvider.GetRequiredService<ISimpleOrchestrationManager>().SendEventToAsync(id, eventName, eventToSend);
        }

        public async Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default) where T : IEvent
        {
            var channel = GetChannel(eventName);

            var eventData = await channel.Reader.ReadAsync(cancellationToken);

            return (T)eventData;
        }

        private Channel<IEvent> GetChannel(string eventName)
        {
            lock (this.eventQueues)
            {
                if (!this.eventQueues.TryGetValue(eventName, out var channel))
                {
                    channel = Channel.CreateUnbounded<IEvent>();
                    this.eventQueues.Add(eventName, channel);
                }
                return channel;
            }
        }

        internal async Task SendEventInternal<T>(string id, string eventName, T eventToSend) where T : IEvent
        {
            var channel = GetChannel(eventName);

            await channel.Writer.WriteAsync(eventToSend);
        }
    }
}
