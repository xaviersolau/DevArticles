using DurableFunctionApp1.Business;
using DurableLib;
using DurableLib.Simple;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableConsole
{
    public class AnotherOrchestrationSubClientFactory : ISubOrchestrationFactory<IAnotherOrchestration>
    {
        private readonly IServiceProvider serviceProvider;

        public AnotherOrchestrationSubClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetSubOrchestrationObject(IOrchestrationContext context)
        {
            return this.serviceProvider.GetRequiredService<IAnotherOrchestration>();
        }
    }

    public class AnotherOrchestrationClientFactory : IOrchestrationFactory<IAnotherOrchestration>
    {
        private readonly IServiceProvider serviceProvider;

        public AnotherOrchestrationClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> NewOrchestrationAsync(IOrchestrationClient client, Func<IAnotherOrchestration, Task> action)
        {
            var orchestrationCtx = this.serviceProvider.GetRequiredService<OrchestrationCtx>();

            var context = new OrchestrationContextSimple(this.serviceProvider);

            orchestrationCtx.SetContext(context);
            try
            {
                var orchestration = this.serviceProvider.GetRequiredService<IAnotherOrchestration>();

                await action(orchestration);
            }
            finally
            {
                orchestrationCtx.SetContext(null);
            }

            return "";
        }
    }

    public sealed class AnotherActivityClientFactory : IActivityFactory<IAnotherActivity>
    {
        private readonly IServiceProvider serviceProvider;

        public AnotherActivityClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetActivityObject(IOrchestrationContext context)
        {
            return this.serviceProvider.GetRequiredService<IAnotherActivity>();
        }
    }

    public class MyOrchestrationSubClientFactory : ISubOrchestrationFactory<IMyOrchestration>
    {
        private readonly IServiceProvider serviceProvider;

        public MyOrchestrationSubClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetSubOrchestrationObject(IOrchestrationContext context)
        {
            return this.serviceProvider.GetRequiredService<IOrchestrationContext>();
        }
    }

    public class MyOrchestrationClientFactory : IOrchestrationFactory<IMyOrchestration>
    {
        private readonly IServiceProvider serviceProvider;

        public MyOrchestrationClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> NewOrchestrationAsync(IOrchestrationClient client, Func<IMyOrchestration, Task> action)
        {
            var orchestrationCtx = this.serviceProvider.GetRequiredService<OrchestrationCtx>();

            var context = new OrchestrationContextSimple(this.serviceProvider);

            orchestrationCtx.SetContext(context);
            try
            {
                var orchestration = this.serviceProvider.GetRequiredService<IMyOrchestration>();

                await action(orchestration);
            }
            finally
            {
                orchestrationCtx.SetContext(null);
            }

            return "";
        }
    }
    public sealed class MyActivitiesClientFactory : IActivityFactory<IMyActivities>
    {
        private readonly IServiceProvider serviceProvider;

        public MyActivitiesClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetActivityObject(IOrchestrationContext context)
        {
            return this.serviceProvider.GetRequiredService<IMyActivities>();
        }
    }

}
