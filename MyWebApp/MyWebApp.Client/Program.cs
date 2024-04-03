using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyWebApp.Client.Localizer;
using SoloX.BlazorJsonLocalization.WebAssembly;
using SoloX.BlazorJsonLocalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Since our localization files are HTTP assets, we need to inject the Host HttpClient
builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add services to enable Json localization.
builder.Services
    .AddWebAssemblyJsonLocalization(builder =>
    {
        builder
            .UseHttpHostedJson(options =>
            {
                // Setup the HTTP main static assets contributor assemblies.
                // Here we only need the Client assembly because the Client won't use
                // any localization from the server side.
                options.ApplicationAssembly = typeof(MyWebApp.Client._Imports).Assembly;
            });
    });

builder.Services
    // Register the client global localization.
    .AddTransient<IClientGlobalStringLocalizer, ClientGlobalStringLocalizer>();

// Load localization resources asynchronously
// replacing :
//
//await builder.Build().RunAsync();
//
// with
//
var webAssemblyHost = builder.Build();

// Get the client localizer.
var localizer = webAssemblyHost.Services.GetRequiredService<IClientGlobalStringLocalizer>();

// Load resources.
await localizer.LoadAsync();

// And run the application.
await webAssemblyHost.RunAsync();
