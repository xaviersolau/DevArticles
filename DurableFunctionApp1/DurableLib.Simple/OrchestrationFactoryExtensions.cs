using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public static class OrchestrationFactoryExtensions
    {
        public static Task<string> NewOrchestrationAsync<TOrchestration>(
            this IOrchestrationFactory<TOrchestration> orchestrationFactory,
            Func<TOrchestration, Task> action)
        {
            var orchestrationClient = new OrchestrationClientSimple();
            return orchestrationFactory.NewOrchestrationAsync(orchestrationClient, action);
        }
    }
}
