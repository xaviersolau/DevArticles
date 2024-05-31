using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System.Threading.Tasks;
using DurableLib.InProc;
using DurableLib.Abstractions;
using FctCodeGen.Patterns.Cls;

namespace FctCodeGen.Patterns.Impl
{
    [Pattern<ClassBasedOnSelector<IEvent>>]
    [Repeat(Pattern = nameof(EventPattern))]
    public static class EventPatternInProcessActivityFunction
    {
        [FunctionName("SendEventInternal" + nameof(EventPattern))]
        public static Task SendEventFunction([ActivityTrigger] SendEventPayload<EventPattern> payload, [DurableClient] IDurableOrchestrationClient client)
        {
            return client.RaiseEventAsync(payload.InstanceId, payload.EventName, payload.Event);
        }
    }
}
