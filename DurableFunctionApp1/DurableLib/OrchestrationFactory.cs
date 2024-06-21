using DurableLib.Abstractions;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationFactory<TOrchestration, TClient> : IOrchestrationFactory<TOrchestration>
        where TClient : OrchestrationClientBase, TOrchestration, new()
        where TOrchestration : IOrchestration
    {
        public async Task<string> NewOrchestrationAsync(IOrchestrationClient client, Func<TOrchestration, Task> action)
        {
            var myOrchestration = new TClient()
            {
                Client = client,
            };

            await action(myOrchestration);

            if (string.IsNullOrEmpty(myOrchestration.Id))
            {
                throw new NotSupportedException($"Could not retrieve instance Id.");
            }

            return myOrchestration.Id;
        }
    }
}
