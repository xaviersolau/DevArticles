using Microsoft.DurableTask;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class ActivityFactory<TActivity, TClient> : IActivityFactory<TActivity>
        where TClient : ActivityClientBase, TActivity, new()
    {
        public object GetActivityObject(TaskOrchestrationContext context)
        {
            var client = new TClient()
            {
                Context = context,
            };

            return client;
        }
    }
}
