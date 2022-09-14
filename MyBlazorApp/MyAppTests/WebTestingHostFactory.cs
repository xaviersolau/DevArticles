using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyAppTests
{
    public class WebTestingHostFactory<TProgram>
  : WebApplicationFactory<TProgram>
  where TProgram : class
    {
        // Override the CreateHost to build our HTTP host server.
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Create the host that is actually used by the
            // TestServer (In Memory).
            var testHost = base.CreateHost(builder);
            // configure and start the actual host using Kestrel.
            builder.ConfigureWebHost(
              webHostBuilder => webHostBuilder.UseKestrel());
            var host = builder.Build();
            host.Start();
            // In order to cleanup and properly dispose HTTP server
            // resources we return a composite host object that is
            // actually just a way to intercept the StopAsync and Dispose
            // call and relay to our HTTP host.
            return new CompositeHost(testHost, host);
        }

        // Relay the call to both test host and kestrel host.
        public class CompositeHost : IHost
        {
            private readonly IHost testHost;
            private readonly IHost kestrelHost;
            public CompositeHost(IHost testHost, IHost kestrelHost)
            {
                this.testHost = testHost;
                this.kestrelHost = kestrelHost;
            }
            public IServiceProvider Services => this.testHost.Services;
            public void Dispose()
            {
                this.testHost.Dispose();
                this.kestrelHost.Dispose();
            }
            public async Task StartAsync(
              CancellationToken cancellationToken = default)
            {
                await this.testHost.StartAsync(cancellationToken);
                await this.kestrelHost.StartAsync(cancellationToken);
            }
            public async Task StopAsync(
              CancellationToken cancellationToken = default)
            {
                await this.testHost.StopAsync(cancellationToken);
                await this.kestrelHost.StopAsync(cancellationToken);
            }
        }
    }
}
