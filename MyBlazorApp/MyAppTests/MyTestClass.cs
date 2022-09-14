using Microsoft.AspNetCore.Hosting;
using MyBlazorApp.Server.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace MyAppTests;

/// <summary>
/// The test class that is using the PlaywrightFixture
/// </summary>
[Collection(PlaywrightFixture.PlaywrightCollection)]
public class MyTestClass
{
    private readonly PlaywrightFixture playwrightFixture;
    /// <summary>
    /// Setup test class injecting a playwrightFixture instance.
    /// </summary>
    /// <param name="playwrightFixture">The playwrightFixture
    /// instance.</param>
    public MyTestClass(PlaywrightFixture playwrightFixture)
    {
        this.playwrightFixture = playwrightFixture;
    }

    [Fact]
    public async Task MyFirstTest()
    {
        var url = "https://localhost:5000";

        // Create the host factory with the App class as parameter and the
        // url we are going to use.
        using var hostFactory = new WebTestingHostFactory<WeatherForecastController>();
        hostFactory
          // Override host configuration to mock stuff if required.
          .WithWebHostBuilder(builder =>
          {
              // Setup the url to use.
              builder.UseUrls(url);
              // Replace or add services if needed.
              builder.ConfigureServices(services =>
              {
                  // services.AddTransient<....>();
              })
              // Replace or add configuration if needed.
              .ConfigureAppConfiguration((app, conf) =>
              {
                  // conf.AddJsonFile("appsettings.Test.json");
              });
          })
          // Create the host using the CreateDefaultClient method.
          .CreateDefaultClient();

        // Open a page and run test logic.
        await this.playwrightFixture.GotoPageAsync(
          url,
          async (page) =>
          {
              // Apply the test logic on the given page.

              // Click text=Home
              await page.Locator("text=Home").ClickAsync();
              await page.WaitForURLAsync($"{url}/");
              // Click text=Hello, world!
              await page.Locator("text=Hello, world!").IsVisibleAsync();

              // Click text=Counter
              await page.Locator("text=Counter").ClickAsync();
              await page.WaitForURLAsync($"{url}/counter");
              // Click h1:has-text("Counter")
              await page.Locator("h1:has-text(\"Counter\")").IsVisibleAsync();
              // Click text=Click me
              await page.Locator("text=Click me").ClickAsync();
              // Click text=Current count: 1
              await page.Locator("text=Current count: 1").IsVisibleAsync();
              // Click text=Click me
              await page.Locator("text=Click me").ClickAsync();
              // Click text=Current count: 2
              await page.Locator("text=Current count: 2").IsVisibleAsync();

          },
          Browser.Chromium);
    }
}