using Microsoft.DurableTask;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableLib
{
    public class OrchestrationFunctionBase
    {
        private readonly IServiceProvider serviceProvider;

        public OrchestrationFunctionBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected Task<TReturn> InternalRunOrchestrationAsync<TOrchestration, TReturn>(TaskOrchestrationContext context, Func<TOrchestration, Task<TReturn>> runHandler)
            where TOrchestration : notnull
        {
            var orchestrationCtx = this.serviceProvider.GetRequiredService<OrchestrationCtx>();

            orchestrationCtx.SetContext(context);
            try
            {
                var orchestration = this.serviceProvider.GetRequiredService<TOrchestration>();

                return runHandler(orchestration);
            }
            finally
            {
                orchestrationCtx.SetContext(null);
            }
        }
    }
}
