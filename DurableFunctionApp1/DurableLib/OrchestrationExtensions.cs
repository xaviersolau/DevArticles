using DurableLib.Abstractions;
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
            var factoryToRegister = new List<Type>();
            var activityFactoryMap = new Dictionary<Type, Type>();
            var options = new OrchestrationOptions(services, activityFactoryMap, factoryToRegister);
            builder(options);

            services.AddScoped(sp => new OrchestrationCtx(activityFactoryMap));

            services.AddTransient(typeof(IEventHub<>), typeof(EventHub<>));

            var assemblies = options.LookupAssemblies ?? AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsGenericType || type.IsInterface)
                    {
                        continue;
                    }

                    foreach (var factoryInterfaceToRegister in factoryToRegister)
                    {
                        if (type.IsAssignableTo(factoryInterfaceToRegister))
                        {
                            services.AddTransient(factoryInterfaceToRegister, type);
                        }
                    }
                }
            }

            return services;
        }
    }
}
