# DevArticles

---

# Easy .NET Web App Integration Testing with Playwright

## Integration Testing Between the Frontend and BFF

In many web applications, the frontend relies on a Backend-for-Frontend (BFF) layer to manage data,
handle API calls, and streamline communication with backend services. Integration testing between
the frontend and BFF ensures that these components work seamlessly together, helping to catch issues
with data flow and user interactions early on.

However, integration testing can be complex, especially when the frontend and backend are built on
different technology stacks. Often, testing requires starting the host, mocking backend services,
and ensuring that all components communicate properly.

[Playwright](https://playwright.dev/dotnet/) offers a streamlined solution that is both
cross-platform and compatible with various development environments, including JavaScript,
TypeScript, Python, Java, and .NET. Yet, even with Playwright, the setup process can be a challenge,
potentially consuming hours before you even begin writing your first test. I am not even talking
about integrating it into your CI/CD pipeline.

This article focuses on a scenario where a .NET backend supports your web application, and you want
to perform integration tests across both the frontend and backend.

---

## The Playwright test builder

With the [Playwright test builder](https://github.com/xaviersolau/CodeQuality), testing your .NET
hosted web application has never been easier. The builder automates the setup process, handling
host startup, Playwright engine configuration, and dependency installation, so you can focus on
writing your tests.

In addition, if you need to run your tests in parallel, the Playwright Test Builder configures
network resources to ensure isolated and concurrent test execution.

This automation allows you to write tests with minimal setup, reducing friction and enhancing
productivity.

In the following sections, I'll guide you through setting up Playwright for a
[Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) application. I'll then explain
how these principles apply to other frameworks, especially [React](https://react.dev/).

> Note that the process is similar for [Angular](https://angular.dev/).

----

## Set up the solution

Now, we set up the solution with a Web Application and a test project for the purposes of this
article.

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

The namespace of the PlaywrightTestBuilder is `SoloX.CodeQuality.Playwright`, so don't forget to add
it to the default namespace. Alternatively, you can use the using statement in your test files.

----

## Write the tests

Now that we have the solution ready, we can start writing a test.

Let's create a basic test class `WebAppTests` that is going to test that the Web Application is
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

> Do not forget the `await using` since `IPlaywrightTest` is a `IAsyncDisposable`.

```csharp
// Create the playwright test.
await using var playwrightTest = await builder.BuildAsync();

// Now we can use the PlaywrightTest and navigate to the page to test.
await playwrightTest.GotoPageAsync(string.Empty, async page =>
{
    var body = page.Locator("body");

    await body.WaitForAsync();
});
```

----

## Run the tests

The first time you run the tests, it may take several minutes as Playwright needs to deploy all its
tooling and dependencies. It is only processed once and it will detect the platform it is running
on.

You can run the test as usual using the `dotnet test` command and it will also work on your CI!

Finally, you can run your tests in parallel. The PlaywrightTestBuilder will set up the host so that
it will be specific to the test and it will use a dedicated network port isolating all tests to
prevent conflicts.

----

## Apply to other frameworks

Now I will adapt the Blazor example using React and it would be the same for Angular.

First, add the React projects. I assume all React required tooling (like node.js, npm...) are
installed on your environment and that you are in the solution folder `DemoTestBuilder`:

```bash
dotnet new react --output AppReact
dotnet sln add AppReact

dotnet new xunit --output AppReactTests
dotnet sln add AppReactTests

dotnet add AppReactTests/AppReactTests.csproj reference AppReact/AppReact.csproj

cd AppReactTests
dotnet add package SoloX.CodeQuality.Playwright
```

From this point, with all React project ready, we can write our test like in the Blazor example.
Unfortunately, it won't work directly because the React template is using a SpaProxy while running
your application from your development environment.

In order to be able to test it, we need to build the React application package in a dedicated build
step in the test project.

Here is the target we need to add in the `AppReactTests.csproj`:

```xml
  <Target Name="BuildReactWebApp" BeforeTargets="Build">
	  <!-- Use npm run build to generate the React application package. -->
	  <Exec WorkingDirectory="../AppReact/ClientApp" Command="npm run build"></Exec>

	  <!-- Select all files form the package. -->
	  <ItemGroup>
		  <BuildItems Include="../AppReact/ClientApp/build/**/*.*" />
	  </ItemGroup>

	  <!-- And Copy the package in the test output folder. -->
	  <Copy SourceFiles="@(BuildItems)" DestinationFolder="$(OutputPath)\build\%(RecursiveDir)"></Copy>
  </Target>
```

Now in the test builder we need to set up the host to base its WebRoot folder to the one we have
just generated.

```csharp
localHostBuilder
    // It tells that we use a local application and we provide a type
    // defined in the application assembly entry point (like Program, App
    // or any type defined in the host assembly).
    .UseApplication<AppReact.WeatherForecast>()
    // Since the test builder is using the WebApplicationFactory, we can
    // use the IWebHostBuilder.
    .UseWebHostBuilder(webHostBuilder =>
    {
        // Get the current project output.
        var webAppPath = Path.GetDirectoryName(typeof(WebAppTests).Assembly.Location);

        // Tells the web host the path where to read the web root static assets.
        webHostBuilder.UseWebRoot(Path.Combine(webAppPath!, "build"));

        // Apply as previously the settings and/or services overrides.
        // ...
    });
```

You can now run the test!

----

## Final Thoughts

As you can see, it is really easy to set up a Playwright test for your .NET hosted Web Application.
It streamlines the entire testing process by managing dependencies, configuring the test environment,
and supporting parallel execution. Whether you're using Blazor, Angular, or React,
PlaywrightTestBuilder offers a versatile solution that allows you to focus on writing tests, not on
managing infrastructure.

I encourage you to give the PlaywrightTestBuilder a try, and I hope you will find it very useful. If
you want to find out more, you can take a look at the
[project repository](https://github.com/xaviersolau/CodeQuality).

Additionally, to view the full code for this example, visit the
[example repository](https://github.com/xaviersolau/DevArticles/tree/demo_test_builder).

Happy coding!
