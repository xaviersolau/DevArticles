using Microsoft.DurableTask;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public interface IActivityFactory
    {
        object GetActivitiesObject(TaskOrchestrationContext context);
    }
}
