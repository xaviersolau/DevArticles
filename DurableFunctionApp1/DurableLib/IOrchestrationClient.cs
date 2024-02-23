using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib
{
    public interface IOrchestrationClient
    {
        Task<string> ScheduleNewOrchestrationInstanceAsync<TInput>(string name, TInput input)
            where TInput : class;
    }
}
