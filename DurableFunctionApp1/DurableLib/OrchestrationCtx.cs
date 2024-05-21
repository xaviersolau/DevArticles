using DurableLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationCtx
    {
        private IOrchestrationContext? context;
        private Dictionary<Type, Type> activityFactoryMap;

        public OrchestrationCtx(Dictionary<Type, Type> activityFactoryMap)
        {
            this.activityFactoryMap = activityFactoryMap;
        }

        public void SetContext(IOrchestrationContext? context)
        {
            this.context = context;
        }

        public object? GetOrchestrationService(IServiceProvider serviceProvider, Type serviceType)
        {
            if (context == null)
            {
                throw new NotSupportedException($"context should not be null.");
            }

            if (serviceType == typeof(ILogger))
            {
                return context.CreateReplaySafeLogger();
            }
            else if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(ILogger<>))
            {
                var argType = serviceType.GenericTypeArguments.Single();
                var logger = context.CreateReplaySafeLogger(argType);

                return Activator.CreateInstance(typeof(OrchestrationLogger<>).MakeGenericType(argType), logger);
            }
            else if (serviceType == typeof(IOrchestrationTools))
            {
                return context.GetOrchestrationTools();
            }
            else if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEventHub<>))
            {
                var eventHub = (EventHub)serviceProvider.GetService(serviceType);

                eventHub.OrchestrationTools = context.GetOrchestrationTools();

                return eventHub;
            }
            else if (activityFactoryMap.TryGetValue(serviceType, out var factoryType))
            {
                var activityFactory = (IActivityFactory)serviceProvider.GetRequiredService(factoryType);

                return activityFactory.GetActivityObject(context);
            }

            return serviceProvider.GetService(serviceType);
        }
    }
}
