namespace DurableLib.Abstractions
{
    public interface IOrchestrationTools
    {
        Task SendEvent<T>(string id, string eventName, T eventToSend) where T : IEvent;

        Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default(CancellationToken)) where T : IEvent;
    }

    public interface IEvent
    {
        //string Name { get; }
    }
}
