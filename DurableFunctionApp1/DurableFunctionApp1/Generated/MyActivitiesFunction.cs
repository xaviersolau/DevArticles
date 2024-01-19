using DurableFunctionApp1.Business;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunctionApp1.Generated
{

    /// <summary>
    /// Generated
    /// </summary>
    public class MyActivitiesFunction
    {
        [Function(nameof(IMyActivities.SayHello))]
        public static Task<string> SayHelloFunction([ActivityTrigger] string name, FunctionContext executionContext)
        {
            var activities = executionContext.InstanceServices.GetRequiredService<IMyActivities>();

            return activities.SayHello(name);
        }
    }
}
