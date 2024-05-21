﻿
using DurableLib.Abstractions;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableLib.Isolated
{
    public class OrchestrationContextIsolated : IOrchestrationContext, IOrchestrationTools
    {
        private TaskOrchestrationContext context;

        public OrchestrationContextIsolated(TaskOrchestrationContext context)
        {
            this.context = context;
        }

        public Task<TResult> CallActivityAsync<TResult, TPayload>(string name, TPayload payload)
        {
            return this.context.CallActivityAsync<TResult>(name, payload);
        }

        public ILogger CreateReplaySafeLogger()
        {
            return this.context.CreateReplaySafeLogger(this.context.Name);
        }

        public ILogger CreateReplaySafeLogger(Type type)
        {
            return this.context.CreateReplaySafeLogger(type);
        }

        public IOrchestrationTools GetOrchestrationTools()
        {
            return this;
        }

        public Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default)
        {
            return this.context.WaitForExternalEvent<T>(eventName, cancellationToken);
        }
    }
}