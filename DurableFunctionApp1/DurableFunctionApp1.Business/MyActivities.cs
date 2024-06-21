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

        public Task<string> SayHello(string name, string comment)
        {
            logger.LogInformation("Saying hello to {name} with comment {comment}.", name, comment);

            //throw new NotImplementedException();

            return Task.FromResult($"Hello {name} with {comment}!");
        }
    }
}
