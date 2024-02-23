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
}
