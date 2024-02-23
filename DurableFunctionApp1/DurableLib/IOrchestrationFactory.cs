namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public interface IOrchestrationFactory<TOrchestration>
    {
        Task<string> NewOrchestrationAsync(IOrchestrationClient client, Func<TOrchestration, Task> action);
    }
}
