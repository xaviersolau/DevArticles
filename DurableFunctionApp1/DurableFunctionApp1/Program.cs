using DurableLib;
using DurableFunctionApp1.Business;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DurableFunctionsMonitor.DotNetIsolated;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.UseDurableFunctionsMonitor((opt, ep) =>
        {
            opt.DisableAuthentication = true;
        });
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddOrchestration(
            options => {
                options.LookupAssemblies = [typeof(Program).Assembly];

                options
                    .UseOrchestration<IMyOrchestration, MyOrchestration>()
                    .UseOrchestration<IAnotherOrchestration, AnotherOrchestration>()
                    .UseActivity<IMyActivities, MyActivities>()
                    .UseActivity<IAnotherActivity, AnotherActivity>();
                });
    })
    .Build();

host.Run();
