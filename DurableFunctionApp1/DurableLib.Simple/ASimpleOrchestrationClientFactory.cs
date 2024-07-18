using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public abstract class ASimpleOrchestrationClientFactory<TOrchestration, TClient> : IOrchestrationFactory<TOrchestration>
        where TClient : SimpleOrchestrationClientBase<TOrchestration>, TOrchestration, new()
        where TOrchestration : notnull
    {
        private readonly IServiceProvider serviceProvider;

        protected ASimpleOrchestrationClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> NewOrchestrationAsync(IOrchestrationClient client, Func<TOrchestration, Task> action)
        {
            var id = Guid.NewGuid().ToString();

            var orchestrationClient = new TClient();

            orchestrationClient.Setup(id, this.serviceProvider);

            await action(orchestrationClient);

            return id.ToString();
        }
    }

    public abstract class SimpleOrchestrationClientBase<TOrchestration>
        where TOrchestration : notnull
    {
        private string id;

        private IServiceProvider serviceProvider = default!;
        private ISimpleOrchestrationManagerInternal simpleOrchestrationManager = default!;

        internal void Setup(string id, IServiceProvider serviceProvider)
        {
            this.id = id;
            this.serviceProvider = serviceProvider;
            this.simpleOrchestrationManager = serviceProvider.GetRequiredService<ISimpleOrchestrationManagerInternal>();
        }

        public async Task<string> RunNewOrchestrationAsync<TPayload, T>(TPayload payload, Expression<Func<TOrchestration, TPayload, Task<T>>> action)
        {
            var instanceId = await simpleOrchestrationManager.RegisterNewOrchestrationAsync(id, this.serviceProvider, payload, action);

            return instanceId;
        }
    }

}
