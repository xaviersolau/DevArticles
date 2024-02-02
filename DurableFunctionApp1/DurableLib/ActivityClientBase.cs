using Microsoft.DurableTask;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public abstract class ActivityClientBase
    {
        public TaskOrchestrationContext Context { get; init; }
    }
}
