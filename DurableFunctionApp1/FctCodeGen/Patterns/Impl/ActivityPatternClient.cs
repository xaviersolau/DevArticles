﻿using DurableLib;
using DurableLib.Abstractions;
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
    public class ActivityPatternClient : ActivityClientBase, IActivityPattern
    {
        [Repeat(Pattern = nameof(IActivityPattern.MethodPattern))]
        [ReplacePattern(typeof(TaskValueTypeReplaceHandler))]
        public Task<ReturnType> MethodPattern([Repeat(Pattern = nameof(argument))] object argument)
        {
            var payload = new ActivityPatternPayload.MethodPatternPayload
            {
                Argument = Repeat.Affectation("argument", argument),
            };

            return Context.CallActivityAsync<ReturnType, ActivityPatternPayload.MethodPatternPayload>(nameof(IActivityPattern) + nameof(IActivityPattern.MethodPattern), payload);
        }
    }
}
