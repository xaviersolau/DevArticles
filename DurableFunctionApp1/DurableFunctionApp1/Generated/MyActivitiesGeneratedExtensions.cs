using DurableFunctionApp1.Business;
using DurableLib;
using Microsoft.Extensions.DependencyInjection;

namespace DurableFunctionApp1.Generated
{
    /// <summary>
    /// Generated
    /// </summary>
    public static class MyActivitiesGeneratedExtensions
    {
        public static IServiceCollection AddMyServicesGenerated(this IServiceCollection services)
        {
            services.AddTransient<IActivityFactory<IMyActivities>,
                ActivitiesFactory<IMyActivities, MyActivitiesClient>>();
            services.AddTransient<IOrchestrationFactory<IMyOrchestration>,
                OrchestrationFactory<IMyOrchestration, MyOrchestrationClient>>();

            return services;
        }
    }
}
