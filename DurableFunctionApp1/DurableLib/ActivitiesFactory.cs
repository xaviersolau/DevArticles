using Microsoft.DurableTask;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class ActivitiesFactory<TActivity, TClient> : IActivityFactory<TActivity>
        where TClient : ActivitiesClientBase, TActivity, new()
    {
        public object GetActivitiesObject(TaskOrchestrationContext context)
        {
            var client = new TClient()
            {
                Context = context,
            };

            return client;
        }
    }
}
