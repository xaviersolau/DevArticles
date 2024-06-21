namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public interface IActivityFactory
    {
        object GetActivityObject(IOrchestrationContext context);
    }

    /// <summary>
    /// From Lib
    /// </summary>
    public interface IActivityFactory<TActivity> : IActivityFactory
    {
    }

    public interface ISubOrchestrationFactory
    {
        object GetSubOrchestrationObject(IOrchestrationContext context);
    }
    public interface ISubOrchestrationFactory<TOrchestration> : ISubOrchestrationFactory
    {
    }
}
