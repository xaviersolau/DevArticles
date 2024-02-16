
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using SoloX.GeneratorTools.Core.CSharp.Generator.Selectors;
using FctCodeGen.Patterns.Impl;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.Model;
using Microsoft.CodeAnalysis;

namespace FctCodeGen
{
    public class FunctionGenerator : IFunctionGenerator
    {
        private readonly IGeneratorLogger<FunctionGenerator> logger;
        private readonly ICSharpWorkspaceFactory workspaceFactory;

        public FunctionGenerator(IGeneratorLogger<FunctionGenerator> logger, ICSharpWorkspaceFactory workspaceFactory)
        {
            this.logger = logger;
            this.workspaceFactory = workspaceFactory;
        }

        public Task Generate(string projectFile)
        {
            var projectFolder = Path.GetDirectoryName(projectFile);

            this.logger.LogInformation($"Loading {Path.GetFileName(projectFile)}...");

            var workspace = this.workspaceFactory.CreateWorkspace();

            var project = workspace.RegisterProject(projectFile);

            var locator = new RelativeLocator(projectFolder, project.RootNameSpace, suffix: "Impl");
            var fileGenerator = new FileWriter(".generated.cs");

            // Generate with a filter on current project interface declarations.
            this.Generate(
                workspace,
                locator,
                fileGenerator,
                workspace.Files);

            return Task.CompletedTask;
        }

        internal void Generate(ICSharpWorkspace workspace, RelativeLocator locator, IWriter fileGenerator, IEnumerable<ICSharpFile> files)
        {
            // Make sure we won't include the pattern files.
            files = files.ToArray();

            workspace.RegisterFile(GetContentFile("./Patterns/Itf/IActivityPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Itf/IOrchestrationPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternClientFactory.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternClientFactory.cs"));

            var resolver = workspace.DeepLoad();

            var generator1 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(ActivityPatternFunction),
                this.logger);

            var generatedItems1 = generator1.Generate(files);

            var generator2 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(ActivityPatternClient),
                this.logger);

            generator2.AddIgnoreUsing("FctCodeGen.Utils");

            var generatedItems2 = generator2.Generate(files);

            var generator3 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(ActivityPatternClientFactory),
                this.logger);

            var generatedItems3 = generator3.Generate(files);

            var generator4 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(OrchestrationPatternFunction),
                this.logger);

            var generatedItems4 = generator4.Generate(files);

            var generator5 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(OrchestrationPatternClient),
                this.logger);

            generator5.AddIgnoreUsing("FctCodeGen.Utils");

            var generatedItems5 = generator5.Generate(files);

            var generator6 = new AutomatedGenerator(
                fileGenerator,
                locator,
                resolver,
                typeof(OrchestrationPatternClientFactory),
                this.logger);

            var generatedItems6 = generator6.Generate(files);
        }

        private static string GetContentFile(string contentFile)
        {
            return Path.Combine(Path.GetDirectoryName(typeof(FunctionGenerator).Assembly.Location)!, contentFile);
        }
    }
}
