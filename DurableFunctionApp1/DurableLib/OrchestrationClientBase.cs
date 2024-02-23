namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationClientBase
    {
        internal IOrchestrationClient Client { get; init; } = default!;

        internal string? Id { get; private set; }

        protected async Task<TReturn> ScheduleNewOrchestrationInstanceAsync<TReturn, TPayload>(string name, TPayload input)
            where TPayload : class
        {
            Id = await Client.ScheduleNewOrchestrationInstanceAsync(name, input);

            return default!;
        }
    }
}
