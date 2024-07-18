using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public abstract class ASimpleOrchestrationSubClientFactory<TOrchestration, TClient> : ISubOrchestrationFactory<TOrchestration>
        where TClient : SimpleOrchestrationSubClientBase<TOrchestration>, TOrchestration, new()
        where TOrchestration : notnull
    {
        private readonly IServiceProvider serviceProvider;

        protected ASimpleOrchestrationSubClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetSubOrchestrationObject(IOrchestrationContext context)
        {
            var subOrchestrationClient = new TClient();

            subOrchestrationClient.Setup(this.serviceProvider);

            return subOrchestrationClient;
        }
    }

    public abstract class SimpleOrchestrationSubClientBase<TOrchestration>
        where TOrchestration: notnull
    {
        private IServiceProvider serviceProvider = default!;
        private ISimpleOrchestrationManagerInternal simpleOrchestrationManager = default!;

        internal void Setup(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.simpleOrchestrationManager = serviceProvider.GetRequiredService<ISimpleOrchestrationManagerInternal>();
        }

        protected Task<T> RunSubOrchestrationAsync<TPayload, T>(TPayload payload, Expression<Func<TOrchestration, TPayload, Task<T>>> handler)
        {
            return simpleOrchestrationManager.RunSubOrchestrationAsync<TOrchestration, TPayload, T>(this.serviceProvider, payload, handler);
        }
    }
}
