using DurableLib.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib.Abstractions
{
    public interface IEventHub<TEvent> where TEvent : IEvent
    {
        Task<TEvent> WaitForEvent(CancellationToken cancellationToken = default(CancellationToken));

        Task SendEvent(string id, TEvent eventToSend);
    }
}
