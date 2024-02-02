using DurableLib;
using DurableLib.Abstractions;
using FctCodeGen.Patterns.Itf;
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
    public sealed class ActivityPatternClientFactory : ActivityFactory<IActivityPattern, ActivityPatternClient>
    {
    }
}
