using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public abstract class ASimpleActivitiesClientFactory<TActivities, TClient> : IActivityFactory<TActivities>
        where TClient : SimpleActivityClientBase<TActivities>, TActivities, new()
        where TActivities : notnull
    {
        private readonly IServiceProvider serviceProvider;

        protected ASimpleActivitiesClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetActivityObject(IOrchestrationContext context)
        {
            var activityClient = new TClient();

            activityClient.Setup(this.serviceProvider);

            return activityClient;
        }
    }

    public abstract class SimpleActivityClientBase<TActivities>
        where TActivities : notnull
    {
        private IServiceProvider serviceProvider = default!;
        private ISimpleOrchestrationManagerInternal simpleOrchestrationManager = default!;

        internal void Setup(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.simpleOrchestrationManager = serviceProvider.GetRequiredService<ISimpleOrchestrationManagerInternal>();
        }

        protected Task<TReturn> RunActivityAsync<TPayload, TReturn>(TPayload payload, Expression<Func<TActivities, TPayload, Task<TReturn>>> handler)
        {
            return simpleOrchestrationManager.RunActivityAsync<TActivities, TPayload, TReturn>(this.serviceProvider, payload, handler);
        }

    }
}
