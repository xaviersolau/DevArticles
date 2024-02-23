using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.GeneratorTools.Core.CSharp.Extensions;

namespace FctCodeGen
{
    internal class Program
    {
        private readonly IFunctionGenerator functionGenerator;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var services = new ServiceCollection();

            services.AddCSharpToolsGenerator();
            services.AddTransient<IFunctionGenerator, FunctionGenerator>();
            services.AddTransient<Program>();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyyMMdd HH:mm:ss:ffff ";
                });
            });

            await using var serviceProvider = services.BuildServiceProvider();

            var program = serviceProvider.GetRequiredService<Program>();

            await program.RunAsync();
        }

        public Program(IFunctionGenerator functionGenerator)
        {
            this.functionGenerator = functionGenerator;
        }

        private async Task RunAsync()
        {
            await this.functionGenerator.Generate(@"../../../../DurableFunctionApp1/DurableFunctionApp1.csproj");
            await this.functionGenerator.Generate(@"../../../../DurableFunctionInProcApp/DurableFunctionInProcApp.csproj");
        }
    }
}
