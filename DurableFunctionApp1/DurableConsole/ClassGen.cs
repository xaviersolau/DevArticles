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
    public class AnotherOrchestrationSubClientFactory : ASimpleOrchestrationSubClientFactory<IAnotherOrchestration, AnotherOrchestrationSubClient>
    {
        public AnotherOrchestrationSubClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }

    public class AnotherOrchestrationSubClient : SimpleOrchestrationSubClientBase<IAnotherOrchestration>, IAnotherOrchestration
    {
        public async Task<string> RunOrchestrator(string parameter)
        {
            return await RunSubOrchestrationAsync(subOrchestration => subOrchestration.RunOrchestrator(parameter));
        }
    }

    public class AnotherOrchestrationClientFactory : ASimpleOrchestrationClientFactory<IAnotherOrchestration>
    {
        public AnotherOrchestrationClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }

    public sealed class AnotherActivityClientFactory : ASimpleActivitiesClientFactory<IAnotherActivity>
    {
        public AnotherActivityClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }

    public class MyOrchestrationSubClientFactory : ASimpleOrchestrationSubClientFactory<IMyOrchestration, MyOrchestrationSubClient>
    {
        public MyOrchestrationSubClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }

    public class MyOrchestrationSubClient : SimpleOrchestrationSubClientBase<IMyOrchestration>, IMyOrchestration
    {
        public async Task<List<string>> RunOrchestrator(string parameter)
        {
            return await RunSubOrchestrationAsync(subOrchestration => subOrchestration.RunOrchestrator(parameter));
        }
    }

    public class MyOrchestrationClientFactory : ASimpleOrchestrationClientFactory<IMyOrchestration>
    {
        public MyOrchestrationClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }

    public sealed class MyActivitiesClientFactory : ASimpleActivitiesClientFactory<IMyActivities>
    {
        public MyActivitiesClientFactory(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }
    }
}
