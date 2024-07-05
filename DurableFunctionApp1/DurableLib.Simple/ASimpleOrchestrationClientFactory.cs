using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public abstract class ASimpleOrchestrationClientFactory<TOrchestration> : IOrchestrationFactory<TOrchestration>
        where TOrchestration : notnull
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ISimpleOrchestrationManagerInternal simpleOrchestrationManager;

        protected ASimpleOrchestrationClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.simpleOrchestrationManager = serviceProvider.GetRequiredService<ISimpleOrchestrationManagerInternal>();
        }

        public Task<string> NewOrchestrationAsync(IOrchestrationClient client, Func<TOrchestration, Task> action)
        {
            var id = simpleOrchestrationManager.RegisterNewOrchestration(this.serviceProvider, action);

            return Task.FromResult(id);
        }
    }
}
