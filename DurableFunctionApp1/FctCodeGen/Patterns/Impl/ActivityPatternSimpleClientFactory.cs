using DurableLib.Abstractions;
using DurableLib.Simple;
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
    public sealed class ActivityPatternSimpleClientFactory : ASimpleActivitiesClientFactory<IActivityPattern, ActivityPatternSimpleClient>
    {
        public ActivityPatternSimpleClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
