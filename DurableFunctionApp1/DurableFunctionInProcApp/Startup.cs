﻿using DurableLib;
using DurableFunctionApp1.Business;
using DurableFunctionInProcApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableFunctionsMonitor.DotNetBackend;

[assembly: FunctionsStartup(typeof(Startup))]

namespace DurableFunctionInProcApp
{
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Configure builder.
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.AddOrchestration(
                options =>
                {
                    options.LookupAssemblies = new[] { typeof(Startup).Assembly };

                    options
                        .UseOrchestration<IMyOrchestration, MyOrchestration>()
                        .UseActivity<IMyActivities, MyActivities>();
                });


            DfmEndpoint.Setup(new DfmSettings
            {
                DisableAuthentication = true,
            });

        }

        /// <summary>
        /// Configuration entry point.
        /// It is going to load local configuration files in order to get Application Settings server URL.
        /// </summary>
        /// <param name="builder"></param>
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            // TODO
        }

    }
}
