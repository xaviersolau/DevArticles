using DurableLib.Abstractions;
using DurableLib.Simple;
using FctCodeGen.Patterns.Itf;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FctCodeGen.Patterns.Impl
{
    [Pattern<InterfaceBasedOnSelector<IOrchestration>>]
    [Repeat(Pattern = nameof(IOrchestrationPattern), Prefix = "I")]
    public class OrchestrationPatternSimpleSubClient : SimpleOrchestrationSubClientBase<IOrchestrationPattern>, IOrchestrationPattern
    {
        [Repeat(Pattern = nameof(IOrchestrationPattern.MethodPattern))]
        [ReplacePattern(typeof(TaskValueTypeReplaceHandler))]
        public async Task<ReturnType> MethodPattern([Repeat(Pattern = nameof(argument))] object argument)
        {
            var payload = new OrchestrationPatternPayload.MethodPatternPayload
            {
                Argument = Repeat.Affectation("argument", argument),
            };

            return await RunSubOrchestrationAsync<OrchestrationPatternPayload.MethodPatternPayload, ReturnType>(payload, (orchestration, pl) => OrchestrationPatternSimpleClient.MethodPatternInternal(orchestration, pl));
        }
    }
}
