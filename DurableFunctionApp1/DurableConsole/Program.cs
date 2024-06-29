using DurableFunctionApp1.Business;
using DurableLib;
using DurableLib.Simple;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace DurableConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            services.AddOrchestration(
                options => {
                    options.LookupAssemblies = [typeof(Program).Assembly];

                    options
                        .UseOrchestration<IMyOrchestration, MyOrchestration>()
                        .UseOrchestration<IAnotherOrchestration, AnotherOrchestration>()
                        .UseActivity<IMyActivities, MyActivities>()
                        .UseActivity<IAnotherActivity, AnotherActivity>();
                });

            services.AddTransient<Program>();

            await using var serviceProvider = services.BuildServiceProvider();

            var prog = serviceProvider.GetRequiredService<Program>();

            await prog.RunAsync("Hello");
        }

        private readonly IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory;

        public Program(IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory)
        {
            this.myOrchestrationFactory = myOrchestrationFactory;
        }

        private async Task RunAsync(string parameter)
        {
            var instanceId = await this.myOrchestrationFactory
                .NewOrchestrationAsync(o => o.RunOrchestrator(parameter));

            Console.WriteLine($"Instance: {instanceId}");



        }
    }
}
