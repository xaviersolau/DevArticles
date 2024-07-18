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
    [Pattern<InterfaceBasedOnSelector<IOrchestration>>]
    [Repeat(Pattern = nameof(IOrchestrationPattern), Prefix = "I")]
    public class OrchestrationPatternSimpleSubClientFactory : ASimpleOrchestrationSubClientFactory<IOrchestrationPattern, OrchestrationPatternSimpleSubClient>
    {
        public OrchestrationPatternSimpleSubClientFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
