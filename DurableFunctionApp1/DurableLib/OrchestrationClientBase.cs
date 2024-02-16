using Microsoft.DurableTask.Client;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationClientBase
    {
        internal DurableTaskClient Client { get; init; } = default!;

        internal string? Id { get; private set; }

        protected async Task<TReturn> ScheduleNewOrchestrationInstanceAsync<TReturn, TPayload>(string name, TPayload input)
        {
            Id = await Client.ScheduleNewOrchestrationInstanceAsync(name, input);

            return default!;
        }
    }
}
