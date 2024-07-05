using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public abstract class ASimpleActivitiesClientFactory<TActivities> : IActivityFactory<TActivities>
        where TActivities : notnull
    {
        private readonly IServiceProvider serviceProvider;

        protected ASimpleActivitiesClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetActivityObject(IOrchestrationContext context)
        {
            return this.serviceProvider.GetRequiredService<TActivities>()!;
        }
    }
}
