using DurableLib.Abstractions;
using FctCodeGen.Patterns.Itf;
using Microsoft.Azure.Functions.Worker;
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
    public class ActivityPatternIsolatedFunction
    {
        [Repeat(Pattern = nameof(IActivityPattern.MethodPattern))]
        [Function(nameof(IActivityPattern) + nameof(IActivityPattern.MethodPattern))]
        [return: Repeat(Pattern = "argument")]
        public static Task<ReturnType> MethodPatternFunction([ActivityTrigger] ActivityPatternPayload.MethodPatternPayload payload, FunctionContext executionContext)
        {
            var argument = Repeat.Affectation("argument", payload.Argument);

            var activity = executionContext.InstanceServices.GetRequiredService<IActivityPattern>();

            return activity.MethodPattern(Repeat.Argument("argument", argument));
        }
    }
}
