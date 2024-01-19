using Microsoft.Extensions.DependencyInjection;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public static class OrchestrationExtensions
    {
        public static IServiceCollection AddOrchestration(this IServiceCollection services, Action<OrchestrationOptions> builder)
        {
            var activityFactoryMap = new Dictionary<Type, Type>();
            var options = new OrchestrationOptions(services, activityFactoryMap);
            builder(options);

            services.AddScoped(sp => new OrchestrationCtx(activityFactoryMap));

            return services;
        }
    }
}
