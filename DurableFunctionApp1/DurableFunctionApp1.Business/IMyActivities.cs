namespace DurableFunctionApp1.Business
{
    public interface IMyActivities
    {
        Task<string> SayHello(string name);
    }
}
