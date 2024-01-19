using Microsoft.DurableTask;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public abstract class ActivitiesClientBase
    {
        public TaskOrchestrationContext Context { get; init; }
    }
}
