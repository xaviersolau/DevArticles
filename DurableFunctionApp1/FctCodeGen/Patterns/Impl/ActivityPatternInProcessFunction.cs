using DurableLib.Abstractions;
using FctCodeGen.Patterns.Itf;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.DependencyInjection;
using SoloX.GeneratorTools.Core.CSharp.Generator;
using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FctCodeGen.Patterns.Impl
{
    [Pattern<InterfaceBasedOnSelector<IActivity>>]
    [Repeat(Pattern = nameof(IActivityPattern), Prefix = "I")]
    public class ActivityPatternInProcessFunction
    {
        private IActivityPattern activityPattern;

        public ActivityPatternInProcessFunction(IActivityPattern activityPattern)
        {
            this.activityPattern = activityPattern;
        }

        [Repeat(Pattern = nameof(IActivityPattern.MethodPattern))]
        [FunctionName(nameof(IActivityPattern) + nameof(IActivityPattern.MethodPattern))]
        [return: Repeat(Pattern = "argument")]
        public Task<ReturnType> MethodPatternFunction([ActivityTrigger] ActivityPatternPayload.MethodPatternPayload payload)
        {
            var argument = Repeat.Affectation("argument", payload.Argument);

            return this.activityPattern.MethodPattern(Repeat.Argument("argument", argument));
        }
    }
}
