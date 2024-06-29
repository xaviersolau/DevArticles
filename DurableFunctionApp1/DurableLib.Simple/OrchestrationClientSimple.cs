using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Simple
{
    public class OrchestrationClientSimple : IOrchestrationClient
    {
        public Task<string> ScheduleNewOrchestrationInstanceAsync<TInput>(string name, TInput input) where TInput : class
        {
            throw new NotImplementedException();
        }
    }
}
