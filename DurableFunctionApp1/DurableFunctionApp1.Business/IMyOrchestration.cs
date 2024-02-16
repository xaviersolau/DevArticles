using DurableLib.Abstractions;

namespace DurableFunctionApp1.Business
{
    public interface IMyOrchestration: IOrchestration
    {
        Task<List<string>> RunOrchestrator(string parameter);
    }
}
