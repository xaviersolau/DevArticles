using Microsoft.DurableTask.Client;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationFactory<TOrchestration, TClient> : IOrchestrationFactory<TOrchestration>
        where TClient : OrchestrationClientBase, TOrchestration, new()
    {
        public async Task<string> NewOrchestrationAsync(DurableTaskClient client, Func<TOrchestration, Task> action)
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
