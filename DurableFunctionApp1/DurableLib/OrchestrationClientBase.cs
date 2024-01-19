using Microsoft.DurableTask.Client;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationClientBase
    {
        public DurableTaskClient Client { get; init; }

        public string? Id { get; protected set; }
    }
}
