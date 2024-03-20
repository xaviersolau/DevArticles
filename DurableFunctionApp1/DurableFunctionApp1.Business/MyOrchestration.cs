using Microsoft.Extensions.Logging;

namespace DurableFunctionApp1.Business
{
    public class MyOrchestration : IMyOrchestration
    {
        private readonly ILogger<MyOrchestration> logger;
        private readonly IMyActivities myActivities;

        public MyOrchestration(ILogger<MyOrchestration> logger, IMyActivities myActivities)
        {
            this.myActivities = myActivities;
            this.logger = logger;
        }

        public async Task<List<string>> RunOrchestrator(string parameter)
        {
            logger.LogInformation("Saying hello.");

            var outputs = new List<string>();

            logger.LogInformation($"Saying hello to {parameter} from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello(parameter, "comment 1"));

            var tasks = new List<Task<string>>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(myActivities.SayHello(parameter, $"comment parallel {i}"));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                outputs.Add(await task);
            }

            logger.LogInformation($"Saying hello to Tokyo from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello("Tokyo", "comment 2"));

            logger.LogInformation($"Saying hello to Seattle from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello("Seattle", "comment 3"));

            logger.LogInformation($"Saying hello to London from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello("London", "comment 4"));

            return outputs;
        }
    }
}
