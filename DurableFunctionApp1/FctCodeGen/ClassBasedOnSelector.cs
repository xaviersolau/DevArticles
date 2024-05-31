
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using SoloX.GeneratorTools.Core.CSharp.Model;
using Microsoft.CodeAnalysis;

namespace FctCodeGen
{
    /// <summary>
    /// Pattern target selector by interfaces implementing a base interface.
    /// </summary>
    /// <typeparam name="TInterface">Base interface.</typeparam>
    public class ClassBasedOnSelector<TClass> : ISelector
        where TClass : class
    {
        /// <inheritdoc/>
        public IEnumerable<IDeclaration<SyntaxNode>> GetDeclarations(IEnumerable<ICSharpFile> files)
        {
            return files
                .SelectMany(file => file.Declarations.Where(d => d is IClassDeclaration).Cast<IClassDeclaration>())
                .Where(d => d.Extends.Any(a => a.Declaration.FullName == typeof(TClass).FullName));
        }

        /// <inheritdoc/>
        public IEnumerable<IMethodDeclaration> GetMethods(IGenericDeclaration<SyntaxNode> declaration)
        {
            return Array.Empty<IMethodDeclaration>();
        }

        /// <inheritdoc/>
        public IEnumerable<IPropertyDeclaration> GetProperties(IGenericDeclaration<SyntaxNode> declaration)
        {
            return Array.Empty<IPropertyDeclaration>();
        }

        /// <inheritdoc/>
        public IEnumerable<IConstantDeclaration> GetConstants(IGenericDeclaration<SyntaxNode> declaration)
        {
            return Array.Empty<IConstantDeclaration>();
        }
    }
}
