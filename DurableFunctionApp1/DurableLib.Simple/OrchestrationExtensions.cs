using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableLib;

namespace DurableLib.Simple
{
    public static class OrchestrationExtensions
    {
        public static IServiceCollection AddSimpleOrchestration(this IServiceCollection services, Action<OrchestrationOptions> builder)
        {
            return services.AddOrchestration(builder)
                .AddSingleton<SimpleOrchestrationManager, SimpleOrchestrationManager>()
                .AddSingleton<ISimpleOrchestrationManager>(r => r.GetRequiredService<SimpleOrchestrationManager>())
                .AddSingleton<ISimpleOrchestrationManagerInternal>(r => r.GetRequiredService<SimpleOrchestrationManager>());
        }
    }
}
