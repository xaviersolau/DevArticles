using DurableLib.Abstractions;

namespace DurableFunctionApp1.Business
{
    public class HelloEvent : IEvent
    {
        public string Hello { get; set; }

        public string? InstanceId { get; set; }
    }
}
