using Microsoft.DurableTask.Client;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public interface IOrchestrationFactory<TOrchestration>
    {
        Task<string> NewOrchestrationAsync(DurableTaskClient client, Func<TOrchestration, Task> action);
    }
}
