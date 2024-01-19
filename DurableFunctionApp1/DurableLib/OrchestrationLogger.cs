using Microsoft.Extensions.Logging;

namespace DurableLib
{
    /// <summary>
    /// From Lib
    /// </summary>
    public class OrchestrationLogger<T> : ILogger<T>
    {
        private readonly ILogger logger;

        public OrchestrationLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this.logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this.logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            this.logger.Log<TState>(logLevel, eventId, state, exception, formatter);
        }
    }
}
