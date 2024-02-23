using DurableLib;
using DurableLib.Abstractions;
using FctCodeGen.Patterns.Itf;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
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
using Microsoft.Extensions.Logging;
using DurableLib.InProc;

namespace FctCodeGen.Patterns.Impl
{
    [Pattern<InterfaceBasedOnSelector<IOrchestration>>]
    [Repeat(Pattern = nameof(IOrchestrationPattern), Prefix = "I")]
    public class OrchestrationPatternInProcessFunction : OrchestrationFunctionBase
    {
        public OrchestrationPatternInProcessFunction(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        [Repeat(Pattern = nameof(IOrchestrationPattern.MethodPattern))]
        [ReplacePattern(typeof(TaskValueTypeReplaceHandler))]
        [FunctionName(nameof(IOrchestrationPattern) + nameof(IOrchestrationPattern.MethodPattern))]
        [return: Repeat(Pattern = "argument")]
        public Task<ReturnType> MethodPattern([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger logger)
        {
            var payload = context.GetInput<OrchestrationPatternPayload.MethodPatternPayload>();

            var orchestrationContext = new OrchestrationContextInProc(context, logger);

            var argument = Repeat.Affectation("argument", payload.Argument);

            return InternalRunOrchestrationAsync<IOrchestrationPattern, ReturnType>(
                orchestrationContext,
                orchestration => orchestration.MethodPattern(Repeat.Argument("argument", argument)));
        }

        
    }
}
