using Microsoft.Extensions.DependencyInjection;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationOptions
    {
        private readonly IServiceCollection services;
        private readonly IDictionary<Type, Type> activityFactoryMap;
        private readonly IList<Type> factoryToRegister;

        public OrchestrationOptions(IServiceCollection services, IDictionary<Type, Type> activityFactoryMap, IList<Type> factoryToRegister)
        {
            this.services = services;
            this.activityFactoryMap = activityFactoryMap;
            this.factoryToRegister = factoryToRegister;
        }

        public OrchestrationOptions UseOrchestration<TOrchestrationInterface, TOrchestration>()
            where TOrchestrationInterface : class
            where TOrchestration : class, TOrchestrationInterface
        {
            var typeInitializer = typeof(TOrchestration).TypeInitializer ?? typeof(TOrchestration).GetConstructors().FirstOrDefault();

            var ctorParameters = typeInitializer?.GetParameters();

            if (ctorParameters == null || ctorParameters.Length == 0)
            {
                services.AddTransient<TOrchestrationInterface, TOrchestration>();
            }
            else
            {
                services.AddTransient(sp =>
                {
                    var orchestrationCtx = sp.GetRequiredService<OrchestrationCtx>();

                    var arguments = ctorParameters.Select(p => orchestrationCtx.GetService(sp, p.ParameterType)).ToArray();

                    var instance = Activator.CreateInstance(typeof(TOrchestration), arguments) ?? throw new NotSupportedException();
                    return (TOrchestrationInterface)instance;
                });
            }

            factoryToRegister.Add(typeof(IOrchestrationFactory<TOrchestrationInterface>));

            return this;
        }

        public OrchestrationOptions UseActivity<TActivityInterface, TActivity>()
            where TActivityInterface : class
            where TActivity : class, TActivityInterface
        {
            activityFactoryMap.Add(typeof(TActivityInterface), typeof(IActivityFactory<TActivityInterface>));

            factoryToRegister.Add(typeof(IActivityFactory<TActivityInterface>));

            services.AddTransient<TActivityInterface, TActivity>();

            return this;
        }
    }
}
