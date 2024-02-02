using Microsoft.DurableTask;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public interface IActivityFactory
    {
        object GetActivityObject(TaskOrchestrationContext context);
    }

    /// <summary>
    /// From Lib
    /// </summary>
    public interface IActivityFactory<TActivity> : IActivityFactory
    {
    }
}
