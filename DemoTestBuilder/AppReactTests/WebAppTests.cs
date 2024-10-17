using Microsoft.AspNetCore.Hosting;
using SoloX.CodeQuality.Playwright;

namespace AppReactTests;

public class WebAppTests
{
    /// <summary>
    /// Since we want to run the test with different browsers we can use a Theory and inline
    /// the list of browsers.
    /// </summary>
    /// <param name="browser">The browser we are going to use in the test.</param>
    [Theory]
    [InlineData(Browser.Chromium)]
    [InlineData(Browser.Firefox)]
    [InlineData(Browser.Webkit)]
    public async Task IsShouldStartTheApplicationAndServeTheMainPage(Browser browser)
    {
        var webAppPath = Path.GetDirectoryName(typeof(WebAppTests).Assembly.Location);

        // Get a PlaywrightTestBuilder instance.
        var builder = PlaywrightTestBuilder.Create()
            // Tells that we run a local host.
            .WithLocalHost(localHostBuilder =>
            {
                localHostBuilder
                    .UsePortRange(new PortRange(5050, 5051))
                    // It tells that we use a local application and we provide a type
                    // defined in the application assembly entry point (like Program, App
                    // or any type defined in the host assembly).
                    .UseApplication<AppReact.WeatherForecast>()
                    // Since the test builder is using the WebApplicationFactory, we can
                    // use the IWebHostBuilder.
                    .UseWebHostBuilder(webHostBuilder =>
                    {
                        // Specify some settings
                        webHostBuilder.UseSetting("MySettingKey", "MySettingValue");
                        //webHostBuilder.UseSetting("", "");

                        webHostBuilder.UseWebRoot(Path.Combine(webAppPath!, "build"));

                        // Specify some service mocks
                        webHostBuilder.ConfigureServices(services =>
                        {
                            // services.AddTransient<IMyService, MyServiceMock>();
                        });
                    });
            });

        builder = builder
            // Allows you to set up Playwright options.
            .WithPlaywrightOptions(opt =>
            {
                // Tells that we want the browser to be displayed on the screen.
                opt.Headless = false;
            })
            // Allows you to set up Playwright New Context options.
            .WithPlaywrightNewContextOptions(opt =>
            {
                // Tells that the viewport size will be 1000x800
                opt.ViewportSize = new Microsoft.Playwright.ViewportSize()
                { Width = 1000, Height = 800 };
            });

        builder = builder
            // Enable traces.
            .WithTraces(tracesConfiguration =>
            {
                // Save traces in the given file.
                tracesConfiguration.UseOutputFile($"traces_{browser}.zip");
            });

        // Create the playwright test.
        await using var playwrightTest = await builder.BuildAsync();

        // Now he can use the PlaywrightTest and navigate to the page to test.
        await playwrightTest.GotoPageAsync(string.Empty, async page =>
        {
            var body = page.Locator("body");

            await body.WaitForAsync();
        });
    }
}