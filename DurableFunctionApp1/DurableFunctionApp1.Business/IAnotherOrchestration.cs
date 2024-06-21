using DurableLib.Abstractions;

namespace DurableFunctionApp1.Business
{
    public interface IAnotherOrchestration : IOrchestration
    {
        Task<string> RunOrchestrator(string parameter);
    }

    public interface IAnotherActivity : IActivity
    {
        Task<string> SayHello(string name);
    }
}
