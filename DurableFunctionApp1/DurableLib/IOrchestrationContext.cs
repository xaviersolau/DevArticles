using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib
{
    public interface IOrchestrationContext
    {

        ILogger CreateReplaySafeLogger();
        ILogger CreateReplaySafeLogger(Type type);

        Task<TResult> CallActivityAsync<TResult, TPayload>(string name, TPayload payload);
    }
}
