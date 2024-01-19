namespace DurableFunctionApp1.Business
{
    public interface IMyOrchestration
    {
        Task<List<string>> RunOrchestrator(string parameter);
    }
}
