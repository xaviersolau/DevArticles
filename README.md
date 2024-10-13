# DevArticles

---

# Easy .NET Web App Integration Testing with Playwright

[Playwright](https://playwright.dev/dotnet/) is a great tool to test your web application. In
addition to being cross-platform, it can be used from many development stacks so you can test
your web application from JavaScript, TypeScript, Python, Java and .NET.

Today, I am going to focus on a scenario where your Web Application is working with a .NET
backend, and you want to perform integration tests for your Web Application testing both your
web frontend and your backend. In that scenario, it may be interesting to start your
application by mocking backend services and running your tests from .NET.
Unfortunately, it can be a pain to set up all you need to be able to do so and you may spend
hours setting up before writing your first test and I am not even talking about your CI.

Thanks to the [Playwright test builder](https://github.com/xaviersolau/CodeQuality), it has
never been so easy to test your .NET hosted web application with Playwright. You can focus on
your test, it will take care of starting your host, the Playwright engine, and installing all
required dependencies.

In addition, if you want to run your tests in parallel, no worries. The Playwright test builder
configures network resources so that your tests are isolated and can run concurrently.

In this article, I am going to give you an example using
[Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) for the Web Application but
as long as your application is .NET hosted, you can be using [Angular](https://angular.dev/),
[React](https://react.dev/) or any other Web technology.

----

## Set up the solution

Now, we will set up the solution with a Web Application and a test project for the purposes
of this article.

First create the solution with the Blazor Web Application project using those commands:

```bash
dotnet new sln --output DemoTestBuilder
cd DemoTestBuilder
dotnet new blazor --output AppBlazor
dotnet sln add AppBlazor
```

Now we can create the test project. Here, I am going to use an xUnit project but you can use any
other testing framework.

```bash
dotnet new xunit --output AppBlazorTests
dotnet sln add AppBlazorTests
```

Finally, we can add a reference to the Web Application project in the created test project.

```bash
dotnet add AppBlazorTests/AppBlazorTests.csproj reference AppBlazor/AppBlazor.csproj
```

----

## Install [PlaywrightTestBuilder](https://www.nuget.org/packages/SoloX.CodeQuality.Playwright) package

In order to use the PlaywrightTestBuilder we need to install the Nuget package:

```bash
cd AppBlazorTests
dotnet add package SoloX.CodeQuality.Playwright
```

The namespace of the PlaywrightTestBuilder is `SoloX.CodeQuality.Playwright` so don’t forget to
add it to the default namespace or you can also use the using statement in your test file.

----

## Write the tests

Now that we have the solution ready, we can start writing a test.

Let’s create a basic test class `WebAppTests` that is going to test that the Web Application is
working and serving the main page using different browsers.

```csharp
// Don't forget to specify the test builder namespace.
using SoloX.CodeQuality.Playwright;

namespace AppBlazorTests
{
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
            // The test implementation...
        }
    }
}
```

The first step to write the test is to define the way you want to run the host with the builder. In
this step we need to tell where to find the Application entry point assembly specifying a Type
(like Program or App for example) and we will be able to override some app settings and/or some app
services.

> Behind the scene, the PlaywrightTestBuilder is using a WebApplicationFactory so we can access the
> `IWebHostBuilder` to customize and/or override settings and services.

```csharp
// Get a PlaywrightTestBuilder instance.
var builder = PlaywrightTestBuilder.Create()
    // Tells that we run a local host.
    .WithLocalHost(localHostBuilder =>
    {
        localHostBuilder
            // It tells that we use a local application and we provide a type
            // defined in the application entry point assembly (like Program, App
            // or any type defined in the host assembly).
            .UseApplication<AppBlazor.Components.App>()
            // Since the test builder is using the WebApplicationFactory, we can
            // use the IWebHostBuilder.
            .UseWebHostBuilder(webHostBuilder =>
            {
                // Specify some settings
                webHostBuilder.UseSetting("MySettingKey", "MySettingValue");

                // Specify some service mocks
                webHostBuilder.ConfigureServices(services =>
                {
                    // services.AddTransient<IMyService, MyServiceMock>();
                });
            });
    });
```

Additionally, it is possible to specify the Playwright options like:
* Headless to tell if we want the browser to be displayed on the screen;
* SlowMo to slow down the test actions;

And the PlaywrightNewContext options like:
* ViewportSize to specify the viewport size;
* StorageStatePath to specify the json file location where to load the storage
from;

> Note that running your test on CI, the Headless option must not be set to false.

```csharp
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
```

We can also configure the builder to enable trace generation, which can be helpful for debugging
and understanding the behavior of your tests:

```csharp
builder = builder
    // Enable traces.
    .WithTraces(tracesConfiguration =>
    {
        // Save traces in the given file.
        tracesConfiguration.UseOutputFile($"traces_{browser}.zip");
    });
```

> For more information about using traces, check out the
> [Playwright trace viewer introduction](https://playwright.dev/dotnet/docs/trace-viewer-intro).
> You can also explore the [online trace viewer](https://trace.playwright.dev/) for an interactive
> experience.

Once the builder is configured, we can instantiate the PlaywrightTest and run the test. In this
example, we are simply opening the browser to target the host index and verifying that we can locate the
body element within the page.

> Don't forget the `await using` since `IPlaywrightTest` is a `IAsyncDisposable`.

```csharp
// Create the playwright test.
await using var playwrightTest = await builder.BuildAsync();

// Now he can use the PlaywrightTest and navigate to the page to test.
await playwrightTest.GotoPageAsync(string.Empty, async page =>
{
    var body = page.Locator("body");

    await body.WaitForAsync();
});
```

----

## Run the tests

The first time you run the tests, it may take several minutes as Playwright needs to deploy all its
tooling and dependencies. It is only processed once and it will detect the platform it is running on.

You can run the test as usual using the `dotnet test` command and it will also work on your CI!

Finally, you can run your tests in parallel. The PlaywrightTestBuilder will set up the host so
that it will use a dedicated network port isolating all tests to prevent conflicts.

----

## Last words

As you can see, it is really easy to set up a Playwright test for your .NET hosted Web Application.
It streamlines the entire testing process by managing dependencies, configuring the test environment,
and supporting parallel execution. Whether you're using Blazor, Angular, or React,
PlaywrightTestBuilder offers a versatile solution that allows you to focus on writing tests, not on
managing infrastructure.

I encourage you to give a try to the PlaywrightTestBuilder and I hope that you will find it very
useful. If you want to find out more, you can take a look at the
[project repository](https://github.com/xaviersolau/CodeQuality).

Additionally, to view the full code for this example, visit the
[example repository](https://github.com/xaviersolau/DevArticles/tree/demo_test_builder).

Happy coding!
