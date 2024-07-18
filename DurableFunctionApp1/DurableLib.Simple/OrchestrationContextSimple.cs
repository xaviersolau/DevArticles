using DurableLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.ExpressionTools.Transform;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Channels;

namespace DurableLib.Simple
{
    public class OrchestrationContextSimple : IOrchestrationContext, IOrchestrationTools
    {
        private IServiceProvider serviceProvider = default!;

        private Dictionary<string, Channel<IEvent>> eventQueues = new Dictionary<string, Channel<IEvent>>();

        public Task? Task { get; private set; }

        public string Id { get; private set; }

        public OrchestrationContextSimple(string id)
        {
            Id = id;
        }

        internal void InvokeOrchestration<TOrchestration, TPayload, TResult>(IServiceProvider rootServiceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TResult>>> action)
             where TOrchestration : notnull
        {
            Task = InternalInvokeOrchestrationAsync(rootServiceProvider, payload, action);
        }

        internal async Task<TResult> InvokeOrchestrationAsync<TOrchestration, TPayload, TResult>(IServiceProvider rootServiceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TResult>>> action)
            where TOrchestration : notnull
        {
            var task = InternalInvokeOrchestrationAsync(rootServiceProvider, payload, action);

            Task = task;

            return await task;
        }

        internal void RewindOrchestration(IServiceProvider rootServiceProvider)
        {
            // Load all types AssemblyQualifiedName
            // Load Payload
            // Load expression

            // TODO Call with loaded data
            Task = InternalInvokeOrchestrationAsync<string, string, string>(rootServiceProvider, null!, null!);
        }

        private async Task<TResult> InternalInvokeOrchestrationAsync<TOrchestration, TPayload, TResult>(IServiceProvider rootServiceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TResult>>> action)
            where TOrchestration : notnull
        {
            string actionString = action.Serialize();

            string orchestrationType = typeof(TOrchestration).AssemblyQualifiedName!;
            string payloadType = typeof(TPayload).AssemblyQualifiedName!;
            string returnType = typeof(TResult).AssemblyQualifiedName!;

            using var asyncServiceScope = rootServiceProvider.CreateAsyncScope();

            this.serviceProvider = asyncServiceScope.ServiceProvider;

            OrchestrationCtx? orchestrationCtx = null;
            try
            {
                orchestrationCtx = serviceProvider.GetRequiredService<OrchestrationCtx>();

                orchestrationCtx.SetContext(this);

                var orchestration = serviceProvider.GetRequiredService<TOrchestration>();

                var actFct = action.Compile();

                return await actFct(orchestration, payload);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                throw;
            }
            finally
            {
                orchestrationCtx?.SetContext(null);

                serviceProvider = default!;
            }
        }

        internal async Task<TReturn> InvokeActivityAsync<TActivity, TPayload, TReturn>(IServiceProvider rootServiceProvider, TPayload payload, Expression<Func<TActivity, TPayload, Task<TReturn>>> action)
            where TActivity : notnull
        {
            string actionString = action.Serialize();

            string payloadType = typeof(TPayload).FullName!;
            string activityType = typeof(TActivity).FullName!;
            string returnType = typeof(TReturn).FullName!;


            using var asyncServiceScope = rootServiceProvider.CreateAsyncScope();

            try
            {
                var activity = asyncServiceScope.ServiceProvider.GetRequiredService<TActivity>();

                var actFct = action.Compile();

                return await actFct(activity, payload);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                throw;
            }
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
