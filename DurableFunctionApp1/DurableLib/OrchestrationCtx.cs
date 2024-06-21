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
        private Dictionary<Type, Type> subOrchestrationFactoryMap;

        public OrchestrationCtx(Dictionary<Type, Type> activityFactoryMap, Dictionary<Type, Type> subOrchestrationFactoryMap)
        {
            this.activityFactoryMap = activityFactoryMap;
            this.subOrchestrationFactoryMap = subOrchestrationFactoryMap;
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
            else if (activityFactoryMap.TryGetValue(serviceType, out var activityFactoryType))
            {
                var activityFactory = (IActivityFactory)serviceProvider.GetRequiredService(activityFactoryType);

                return activityFactory.GetActivityObject(context);
            }
            else if (subOrchestrationFactoryMap.TryGetValue(serviceType, out var subOrchestrationFactoryType))
            {
                var subOrchestrationFactory = (ISubOrchestrationFactory)serviceProvider.GetRequiredService(subOrchestrationFactoryType);

                return subOrchestrationFactory.GetSubOrchestrationObject(context);
            }

            return serviceProvider.GetService(serviceType);
        }
    }
}
