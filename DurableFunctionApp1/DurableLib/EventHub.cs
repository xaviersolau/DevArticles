using DurableLib.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib
{

    public class EventHub
    {
        internal IOrchestrationTools OrchestrationTools { get; set; }
    }

    public class EventHub<TEvent> : EventHub, IEventHub<TEvent> where TEvent : IEvent
    {
        public Task<TEvent> WaitForEvent(CancellationToken cancellationToken = default)
        {
            return this.OrchestrationTools.WaitForExternalEvent<TEvent>(typeof(TEvent).Name, cancellationToken);
        }
    }
}
