using Microsoft.DurableTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationCtx
    {
        private TaskOrchestrationContext? context;
        private Dictionary<Type, Type> activityFactoryMap;

        public OrchestrationCtx(Dictionary<Type, Type> activityFactoryMap)
        {
            this.activityFactoryMap = activityFactoryMap;
        }

        public void SetContext(TaskOrchestrationContext? context)
        {
            this.context = context;
        }

        public object? GetService(IServiceProvider serviceProvider, Type serviceType)
        {
            if (context == null)
            {
                throw new NotSupportedException($"context should not be null.");
            }

            if (serviceType == typeof(ILogger))
            {
                return context.CreateReplaySafeLogger(context.Name);
            }
            else if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(ILogger<>))
            {
                var argType = serviceType.GenericTypeArguments.Single();
                var logger = context.CreateReplaySafeLogger(argType);

                return Activator.CreateInstance(typeof(OrchestrationLogger<>).MakeGenericType(argType), logger);
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
