using Microsoft.CodeAnalysis;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model.Use;
using SoloX.GeneratorTools.Core.CSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FctCodeGen.Utils
{
    public class ReturnTypeReplaceHandler : IReplacePatternHandlerFactory, IReplacePatternHandler
    {
        private string source = default!;
        private string target = default!;

        /// <inheritdoc/>
        public string ApplyOn(string patternText)
        {
            if (patternText == null)
            {
                throw new ArgumentNullException(nameof(patternText));
            }

            return patternText.Replace(this.source, this.target);
        }

        /// <inheritdoc/>
        public IReplacePatternHandler Setup(IGenericDeclaration<SyntaxNode> pattern, IGenericDeclaration<SyntaxNode> declaration)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (declaration == null)
            {
                throw new ArgumentNullException(nameof(declaration));
            }

            this.source = ((IGenericDeclarationUse)pattern.Extends.First()).GenericParameters.First().Declaration.Name;
            this.target = ((IGenericDeclarationUse)declaration.Extends.First()).GenericParameters.First().Declaration.Name;
            return this;
        }
    }
}
