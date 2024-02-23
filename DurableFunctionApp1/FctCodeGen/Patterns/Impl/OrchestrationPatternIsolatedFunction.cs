using DurableLib;
using DurableLib.Abstractions;
using DurableLib.Isolated;
using FctCodeGen.Patterns.Itf;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.DependencyInjection;
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
    public class OrchestrationPatternIsolatedFunction : OrchestrationFunctionBase
    {
        public OrchestrationPatternIsolatedFunction(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        [Repeat(Pattern = nameof(IOrchestrationPattern.MethodPattern))]
        [ReplacePattern(typeof(TaskValueTypeReplaceHandler))]
        [Function(nameof(IOrchestrationPattern) + nameof(IOrchestrationPattern.MethodPattern))]
        [return: Repeat(Pattern = "argument")]
        public Task<ReturnType> MethodPattern([OrchestrationTrigger] TaskOrchestrationContext context, OrchestrationPatternPayload.MethodPatternPayload payload)
        {
            var orchestrationContext = new OrchestrationContextIsolated(context);

            var argument = Repeat.Affectation("argument", payload.Argument);

            return InternalRunOrchestrationAsync<IOrchestrationPattern, ReturnType>(
                orchestrationContext,
                orchestration => orchestration.MethodPattern(Repeat.Argument("argument", argument)));
        }
    }
}
