using Microsoft.Extensions.Logging;

namespace DurableFunctionApp1.Business
{
    public class MyActivities : IMyActivities
    {
        private readonly ILogger<MyActivities> logger;

        public MyActivities(ILogger<MyActivities> logger)
        {
            this.logger = logger;
        }

        public Task<string> SayHello(string name)
        {
            logger.LogInformation("Saying hello to {name}.", name);

            return Task.FromResult($"Hello {name}!");
        }
    }
}
