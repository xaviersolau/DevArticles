using MyWebApp.Client.Localizer;
using MyWebApp.Components;
using MyWebApp.Localizer;
using SoloX.BlazorJsonLocalization.ServerSide;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Add services to enable Json localization.
builder.Services
    .AddServerSideJsonLocalization(builder =>
    {
        builder
            .UseHttpHostedJson(options =>
            {
                // Set up the HTTP main static assets contributor assemblies.
                // Here we need both the Host assembly and the Client assembly because
                // the Host can render both components from the host side or the client side.
                options.ApplicationAssemblies =
                [
                    typeof(MyWebApp.Client._Imports).Assembly,
                    typeof(MyWebApp.Components.App).Assembly
                ];
            });
    });

builder.Services
    // Register the server global localization.
    .AddTransient<IServerGlobalStringLocalizer, ServerGlobalStringLocalizer>()
    // Since client pages can also be rendered on server side we need to register the
    // client global localization.
    .AddTransient<IClientGlobalStringLocalizer, ClientGlobalStringLocalizer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyWebApp.Client._Imports).Assembly);

app.Run();
