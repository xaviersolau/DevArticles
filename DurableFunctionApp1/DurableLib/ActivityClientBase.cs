namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public abstract class ActivityClientBase
    {
        public IOrchestrationContext Context { get; init; } = default!;
    }

    public abstract class SubOrchestrationClientBase
    {
        public IOrchestrationContext Context { get; init; } = default!;
    }
}
