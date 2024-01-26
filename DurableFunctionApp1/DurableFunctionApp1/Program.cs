using DurableLib;
using DurableFunctionApp1.Generated;
using DurableFunctionApp1.Business;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddOrchestration(
            options => options
                .UseOrchestration<IMyOrchestration, MyOrchestration>()
                .UseActivity<IMyActivities, MyActivities>());
    })
    .Build();

host.Run();