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
    [Pattern<InterfaceBasedOnSelector<IActivity>>]
    [Repeat(Pattern = nameof(IActivityPattern), Prefix = "I")]
    public sealed class ActivityPatternSimpleClient : SimpleActivityClientBase<IActivityPattern>, IActivityPattern
    {
        [Repeat(Pattern = nameof(IActivityPattern.MethodPattern))]
        [ReplacePattern(typeof(TaskValueTypeReplaceHandler))]
        public Task<ReturnType> MethodPattern([Repeat(Pattern = nameof(argument))] object argument)
        {
            var payload = new ActivityPatternPayload.MethodPatternPayload
            {
                Argument = Repeat.Affectation("argument", argument),
            };

            return RunActivityAsync<ActivityPatternPayload.MethodPatternPayload, ReturnType>(payload, (activity, pl) => activity.MethodPattern(Repeat.Argument("argument", argument)));
        }
    }
}
