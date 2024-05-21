namespace DurableLib.Abstractions
{
    public interface IOrchestrationTools
    {
        Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IEvent
    {
        //string Name { get; }
    }
}
