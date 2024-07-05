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

            services.AddSimpleOrchestration(
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
        private readonly ISimpleOrchestrationManager orchestrationManager;

        public Program(IOrchestrationFactory<IMyOrchestration> myOrchestrationFactory, ISimpleOrchestrationManager orchestrationManager)
        {
            this.myOrchestrationFactory = myOrchestrationFactory;
            this.orchestrationManager = orchestrationManager;
        }

        private async Task RunAsync(string parameter)
        {
            var instanceId1 = await this.myOrchestrationFactory
                .NewOrchestrationAsync(o => o.RunOrchestrator(parameter));

            var instanceId2 = await this.myOrchestrationFactory
                .NewOrchestrationAsync(o => o.RunOrchestrator(parameter));

            Console.WriteLine($"Instance: {instanceId1}");
            Console.WriteLine($"Instance: {instanceId2}");


            Console.WriteLine($"Key to send the message to instance: {instanceId1}");
            Console.ReadKey();

            await this.orchestrationManager.SendEventToAsync(instanceId1, "HelloEvent", new HelloEvent
            {
                Hello = "Hello world",
                InstanceId = instanceId2,
            });

            Console.WriteLine($"Waiting end of instance: {instanceId1}");
            await this.orchestrationManager.AwaitOrchestration(instanceId1);
            Console.WriteLine($"Waiting end of instance: {instanceId2}");
            await this.orchestrationManager.AwaitOrchestration(instanceId2);
        }
    }
}
