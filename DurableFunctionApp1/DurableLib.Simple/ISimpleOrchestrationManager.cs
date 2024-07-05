using DurableLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public interface ISimpleOrchestrationManager
    {
        Task AwaitOrchestration(string instanceId);

        Task SendEventToAsync<T>(string instanceId, string eventName, T eventData) where T : IEvent;
    }

    internal interface ISimpleOrchestrationManagerInternal
    {
        string RegisterNewOrchestration<TOrchestration>(IServiceProvider serviceProvider, Func<TOrchestration, Task> action) where TOrchestration : notnull;

        Task<T> RunSubOrchestrationAsync<TOrchestration, T>(IServiceProvider serviceProvider, Func<TOrchestration, Task<T>> action) where TOrchestration : notnull;
    }

    public class SimpleOrchestrationManager : ISimpleOrchestrationManager, ISimpleOrchestrationManagerInternal
    {
        private Dictionary<Guid, OrchestrationInstance> instances = new Dictionary<Guid, OrchestrationInstance>();

        public Task AwaitOrchestration(string instanceId)
        {
            lock (instances)
            {
                var task = instances[Guid.Parse(instanceId)].Task;

                return task ?? Task.CompletedTask;
            }
        }

        public string RegisterNewOrchestration<TOrchestration>(IServiceProvider serviceProvider, Func<TOrchestration, Task> action) where TOrchestration : notnull
        {
            var id = Guid.NewGuid();

            var asyncServiceScope = serviceProvider.CreateAsyncScope();

            var context = new OrchestrationContextSimple(asyncServiceScope.ServiceProvider);

            var task = async () =>
            {
                OrchestrationCtx? orchestrationCtx = null;
                try
                {
                    orchestrationCtx = asyncServiceScope.ServiceProvider.GetRequiredService<OrchestrationCtx>();

                    orchestrationCtx.SetContext(context);

                    var orchestration = asyncServiceScope.ServiceProvider.GetRequiredService<TOrchestration>();

                    await action(orchestration);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    var instance = instances[id];
                    instance.Exception = e;
                }
                finally
                {
                    orchestrationCtx?.SetContext(null);

                    lock (instances)
                    {
                        var instance = instances[id];
                        instance.Task = null;
                        instance.Context = null;
                    }

                    await asyncServiceScope.DisposeAsync();
                }
            };

            var instance = new OrchestrationInstance
            {
                Id = id,
                Task = task.Invoke(),
                Context = context,
            };

            lock (instances)
            {
                instances.Add(id, instance);
            }

            return id.ToString();
        }

        public async Task<T> RunSubOrchestrationAsync<TOrchestration, T>(IServiceProvider serviceProvider, Func<TOrchestration, Task<T>> action) where TOrchestration : notnull
        {
            var id = Guid.NewGuid();

            using var asyncServiceScope = serviceProvider.CreateAsyncScope();

            var context = new OrchestrationContextSimple(asyncServiceScope.ServiceProvider);

            var instance = new OrchestrationInstance
            {
                Id = id,
                Context = context,
            };

            lock (instances)
            {
                instances.Add(id, instance);
            }

            OrchestrationCtx? orchestrationCtx = null;
            try
            {
                orchestrationCtx = asyncServiceScope.ServiceProvider.GetRequiredService<OrchestrationCtx>();

                orchestrationCtx.SetContext(context);

                var orchestration = asyncServiceScope.ServiceProvider.GetRequiredService<TOrchestration>();

                return await action(orchestration);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                instance.Exception = e;

                throw;
            }
            finally
            {
                orchestrationCtx?.SetContext(null);

                lock (instances)
                {
                    instance.Context = null;
                }
            }
        }

        public Task SendEventToAsync<T>(string instanceId, string eventName, T eventData) where T : IEvent
        {
            OrchestrationContextSimple? context;

            lock (instances)
            {
                context = this.instances[Guid.Parse(instanceId)].Context;
            }

            if (context == null)
            {
                throw new InvalidOperationException($"Instance already completed {instanceId}");
            }

            return context.SendEventInternal<T>(instanceId, eventName, eventData);
        }
    }

    public class OrchestrationInstance
    {
        public Guid Id { get; set; }

        public Task? Task { get; set; }

        public OrchestrationContextSimple? Context { get; set; }

        public Exception? Exception { get; internal set; }
    }
}
