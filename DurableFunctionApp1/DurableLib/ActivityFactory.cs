using DurableLib.Abstractions;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class ActivityFactory<TActivity, TClient> : IActivityFactory<TActivity>
        where TClient : ActivityClientBase, TActivity, new()
        where TActivity : IActivity
    {
        public object GetActivityObject(IOrchestrationContext context)
        {
            var client = new TClient()
            {
                Context = context,
            };

            return client;
        }
    }

    /// <summary>
    /// From Lib
    /// </summary>
    public class SubOrchestrationFactory<TOrchestration, TClient> : ISubOrchestrationFactory<TOrchestration>
        where TClient : SubOrchestrationClientBase, TOrchestration, new()
        where TOrchestration : IOrchestration
    {
        public object GetSubOrchestrationObject(IOrchestrationContext context)
        {
            var client = new TClient()
            {
                Context = context,
            };

            return client;
        }
    }
}
