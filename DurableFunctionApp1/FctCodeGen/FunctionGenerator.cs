
using SoloX.GeneratorTools.Core.CSharp.Workspace;
using SoloX.GeneratorTools.Core.Generator.Impl;
using SoloX.GeneratorTools.Core.Generator;
using SoloX.GeneratorTools.Core.Utils;
using SoloX.GeneratorTools.Core.CSharp.Generator.Impl;
using FctCodeGen.Patterns.Impl;
using Microsoft.CodeAnalysis;
using DurableLib.Isolated;
using SoloX.GeneratorTools.Core.CSharp.Model.Resolver;
using DurableLib.InProc;
using DurableLib.Simple;
using SoloX.GeneratorTools.Core.CSharp.Generator;

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
            var projectObjFolder = Path.Combine(projectFolder, "obj");

            this.logger.LogInformation($"Loading {Path.GetFileName(projectFile)}...");

            var workspace = this.workspaceFactory.CreateWorkspace();

            var project = workspace.RegisterProject(projectFile);

            var locator = new RelativeLocator(projectObjFolder, "none", suffix: "Impl");
            var fileGenerator = new FileWriter(".generated.cs");

            // Generate with a filter on current project interface declarations.
            this.Generate(
                workspace,
                locator,
                fileGenerator,
                workspace.Files);

            var targetFile = GetContentFile("./Recources/Orchestration.targets");
            var target = File.ReadAllText(targetFile);
            File.WriteAllText(Path.Combine(projectObjFolder, Path.GetFileName(projectFile) + ".Orchestration.targets"), target);

            return Task.CompletedTask;
        }

        internal void Generate(ICSharpWorkspace workspace, RelativeLocator locator, IWriter fileGenerator, IEnumerable<ICSharpFile> files)
        {
            // Make sure we won't include the pattern files.
            files = files.ToArray();

            workspace.RegisterFile(GetContentFile("./Patterns/Itf/IActivityPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Itf/IOrchestrationPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Cls/EventPattern.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternIsolatedFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternInProcessFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternPayload.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternClientFactory.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternIsolatedFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternInProcessFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternPayload.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternClientFactory.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/EventPatternInProcessActivityFunction.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternSubClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternSubClientFactory.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternSimpleClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/ActivityPatternSimpleClientFactory.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternSimpleClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternSimpleClientFactory.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternSimpleSubClient.cs"));
            workspace.RegisterFile(GetContentFile("./Patterns/Impl/OrchestrationPatternSimpleSubClientFactory.cs"));

            var resolver = workspace.DeepLoad();

            var generationContext = GetGenerationContext(resolver);

            switch (generationContext)
            {
                case GenerationContext.AzureFunctionInProcess:
                    GenerateAzureFunctionInProcess(
                        fileGenerator,
                        locator,
                        resolver,
                        files);
                    break;
                case GenerationContext.AzureFunctionIsolated:
                    GenerateAzureFunctionIsolated(
                        fileGenerator,
                        locator,
                        resolver,
                        files);
                    break;
                case GenerationContext.Simple:
                    GenerateSimple(
                        fileGenerator,
                        locator,
                        resolver,
                        files);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GenerateSimple(IWriter fileGenerator, RelativeLocator locator, IDeclarationResolver resolver, IEnumerable<ICSharpFile> files)
        {
            Generate(typeof(ActivityPatternSimpleClient), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternSimpleClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSimpleClient), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSimpleClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSimpleSubClient), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSimpleSubClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternPayload), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternPayload), fileGenerator, locator, resolver, files);
        }

        private void GenerateAzureFunctionIsolated(IWriter fileGenerator, RelativeLocator locator, IDeclarationResolver resolver, IEnumerable<ICSharpFile> files)
        {
            Generate(typeof(ActivityPatternIsolatedFunction), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternClient), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternIsolatedFunction), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternClient), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSubClient), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSubClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternPayload), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternPayload), fileGenerator, locator, resolver, files);
        }

        private void GenerateAzureFunctionInProcess(IWriter fileGenerator, RelativeLocator locator, IDeclarationResolver resolver, IEnumerable<ICSharpFile> files)
        {
            Generate(typeof(EventPatternInProcessActivityFunction), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternInProcessFunction), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternClient), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternInProcessFunction), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternClient), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSubClient), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternSubClientFactory), fileGenerator, locator, resolver, files);
            Generate(typeof(ActivityPatternPayload), fileGenerator, locator, resolver, files);
            Generate(typeof(OrchestrationPatternPayload), fileGenerator, locator, resolver, files);
        }

        private IEnumerable<IGeneratedItem> Generate(Type patternType, IWriter fileGenerator, RelativeLocator locator, IDeclarationResolver resolver, IEnumerable<ICSharpFile> files)
        {
            var generator1 = new AutomatedGenerator(
                            fileGenerator,
                            locator,
                            resolver,
                            patternType,
                            this.logger);

            return generator1.Generate(files);
        }

        private GenerationContext GetGenerationContext(IDeclarationResolver resolver)
        {
            var isolatedContext = resolver.Find(typeof(OrchestrationContextIsolated).FullName);

            if (isolatedContext != null && isolatedContext.Count() != 0)
            {
                return GenerationContext.AzureFunctionIsolated;
            }

            var inProcessContext = resolver.Find(typeof(OrchestrationContextInProc).FullName);

            if (inProcessContext != null && inProcessContext.Count() != 0)
            {
                return GenerationContext.AzureFunctionInProcess;
            }

            var simpleContext = resolver.Find(typeof(OrchestrationContextSimple).FullName);

            if (simpleContext != null && simpleContext.Count() != 0)
            {
                return GenerationContext.Simple;
            }

            return GenerationContext.Unknown;
        }

        private static string GetContentFile(string contentFile)
        {
            return Path.Combine(Path.GetDirectoryName(typeof(FunctionGenerator).Assembly.Location)!, contentFile);
        }
    }

    public enum GenerationContext
    {
        Unknown,
        AzureFunctionIsolated,
        AzureFunctionInProcess,
        Simple,
    }
}
