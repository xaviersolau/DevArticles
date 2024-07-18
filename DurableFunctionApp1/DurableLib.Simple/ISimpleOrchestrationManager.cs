using DurableLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public interface ISimpleOrchestrationManager
    {
        Task AwaitOrchestration(string instanceId);

        Task<T> AwaitOrchestration<T>(string instanceId);

        Task SendEventToAsync<T>(string instanceId, string eventName, T eventData) where T : IEvent;

        Task RewindAsync(string id, IServiceProvider serviceProvider);
    }

    internal interface ISimpleOrchestrationManagerInternal
    {
        Task<string> RegisterNewOrchestrationAsync<TOrchestration, TPayload, TResult>(string id, IServiceProvider serviceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TResult>>> action)
            where TOrchestration : notnull;

        Task<TReturn> RunSubOrchestrationAsync<TOrchestration, TPayload, TReturn>(IServiceProvider serviceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TReturn>>> action)
            where TOrchestration : notnull;

        Task<TReturn> RunActivityAsync<TActivity, TPayload, TReturn>(IServiceProvider serviceProvider, TPayload payload, Expression<Func<TActivity, TPayload, Task<TReturn>>> action)
            where TActivity : notnull;
    }

    public class SimpleOrchestrationManager : ISimpleOrchestrationManager, ISimpleOrchestrationManagerInternal
    {
        private Dictionary<string, OrchestrationContextSimple> instances = new Dictionary<string, OrchestrationContextSimple>();

        public Task AwaitOrchestration(string instanceId)
        {
            lock (instances)
            {
                instances.TryGetValue(instanceId, out var context);

                if (context == null)
                {
                    throw new InvalidOperationException($"Instance not found {instanceId}");
                }

                if (context.Task == null)
                {
                    throw new InvalidOperationException($"Task not started {instanceId}");
                }

                return context.Task;
            }
        }

        public Task<T> AwaitOrchestration<T>(string instanceId)
        {
            lock (instances)
            {
                instances.TryGetValue(instanceId, out var context);

                if (context == null)
                {
                    throw new InvalidOperationException($"Instance not found {instanceId}");
                }

                var task = context.Task;

                if (task == null)
                {
                    throw new InvalidOperationException($"Task not started {instanceId}");
                }

                if (task is Task<T> typedTask)
                {
                    return typedTask;
                }

                throw new InvalidOperationException($"Unexpected return type {instanceId}: {task.GetType().GenericTypeArguments.FirstOrDefault()}");
            }
        }

        public Task RewindAsync(string id, IServiceProvider serviceProvider)
        {
            OrchestrationContextSimple? context;

            // Build a new Context or get the existing one
            lock (instances)
            {
                if (!instances.TryGetValue(id, out context))
                {
                    context = new OrchestrationContextSimple(id);

                    instances.Add(id, context);
                }
            }

            context.RewindOrchestration(serviceProvider);

            return Task.CompletedTask;
        }

        public Task<string> RegisterNewOrchestrationAsync<TOrchestration, TPayload, TResult>(string id, IServiceProvider serviceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TResult>>> action)
            where TOrchestration : notnull
        {
            if (instances.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            var context = new OrchestrationContextSimple(id);

            lock (instances)
            {
                instances.Add(id, context);
            }

            context.InvokeOrchestration(serviceProvider, payload, action);

            return Task.FromResult(id);
        }

        public Task<TReturn> RunSubOrchestrationAsync<TOrchestration, TPayload, TReturn>(IServiceProvider serviceProvider, TPayload payload, Expression<Func<TOrchestration, TPayload, Task<TReturn>>> action)
            where TOrchestration : notnull
        {
            var id = Guid.NewGuid().ToString();

            var context = new OrchestrationContextSimple(id);

            lock (instances)
            {
                instances.Add(id, context);
            }

            return context.InvokeOrchestrationAsync(serviceProvider, payload, action);
        }

        public Task<TReturn> RunActivityAsync<TActivity, TPayload, TReturn>(IServiceProvider serviceProvider, TPayload payload, Expression<Func<TActivity, TPayload, Task<TReturn>>> action)
            where TActivity : notnull
        {
            var orchestrationCtx = serviceProvider.GetRequiredService<OrchestrationCtx>();

            var context = (OrchestrationContextSimple)orchestrationCtx.GetContext();

            return context.InvokeActivityAsync(serviceProvider, payload, action);
        }

        public Task SendEventToAsync<T>(string instanceId, string eventName, T eventData) where T : IEvent
        {
            OrchestrationContextSimple? context;

            lock (instances)
            {
                this.instances.TryGetValue(instanceId, out context);
            }

            if (context == null)
            {
                throw new InvalidOperationException($"Instance not found {instanceId}");
            }

            if (context.Task != null && context.Task.IsCompleted)
            {
                throw new InvalidOperationException($"Instance already completed {instanceId}");
            }

            return context.SendEventInternal(instanceId, eventName, eventData);
        }
    }
}
