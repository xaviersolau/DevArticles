using DurableFunctionApp1.Business;
using DurableLib;

namespace DurableFunctionApp1.Generated
{
    /// <summary>
    /// Generated
    /// </summary>
    public class MyActivitiesClient : ActivitiesClientBase, IMyActivities
    {
        public Task<string> SayHello(string name)
        {
            return Context.CallActivityAsync<string>(nameof(IMyActivities.SayHello), name);
        }
    }
}
