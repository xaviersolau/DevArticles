using DurableLib.Abstractions;
using Microsoft.Extensions.Logging;

namespace DurableFunctionApp1.Business
{
    public class MyOrchestration : IMyOrchestration
    {
        private readonly ILogger<MyOrchestration> logger;
        private readonly IMyActivities myActivities;
        private readonly IEventHub<HelloEvent> helloEventHub;
        private readonly IAnotherOrchestration anotherOrchestration;

        public MyOrchestration(ILogger<MyOrchestration> logger, IMyActivities myActivities, IEventHub<HelloEvent> helloEventHub, IAnotherOrchestration anotherOrchestration)
        {
            this.myActivities = myActivities;
            this.helloEventHub = helloEventHub;
            this.anotherOrchestration = anotherOrchestration;
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

            logger.LogInformation($"WAITING EVENT HELLO.");

            var helloEvent = await this.helloEventHub.WaitForEvent();

            logger.LogInformation($"Received EVENT HELLO with message {helloEvent.Hello} and instance id {helloEvent.InstanceId}.");

            if (helloEvent.InstanceId != null)
            {
                logger.LogInformation($"SENDING EVENT HELLO.");
                await this.helloEventHub.SendEvent(helloEvent.InstanceId, new HelloEvent() { Hello = helloEvent.Hello });
            }

            logger.LogInformation($"Saying hello to Tokyo from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello("Tokyo", "comment 2"));

            logger.LogInformation($"Saying hello to Seattle from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello("Seattle", "comment 3"));

            logger.LogInformation($"Saying hello to London from RunOrchestrator.");

            outputs.Add(await myActivities.SayHello("London", "comment 4"));

            logger.LogInformation($"Running sub-orchestration.");
            var res = await anotherOrchestration.RunOrchestrator("toto");
            logger.LogInformation($"Sub-orchestration result is {res}.");

            return outputs;
        }
    }

    public class AnotherOrchestration : IAnotherOrchestration
    {
        private readonly ILogger<AnotherOrchestration> logger;
        private readonly IAnotherActivity anotherActivity;

        public AnotherOrchestration(ILogger<AnotherOrchestration> logger, IAnotherActivity anotherActivity)
        {
            this.logger = logger;
            this.anotherActivity = anotherActivity;
        }

        public async Task<string> RunOrchestrator(string parameter)
        {
            this.logger.LogInformation($"RUN ANOTHERORCHESTRATION.");

            await this.anotherActivity.SayHello("step1");


            await this.anotherActivity.SayHello("step2");

            //throw new NotImplementedException("not yet!");

            return $"From AnotherOrchestration {parameter}";
        }
    }

    public class AnotherActivity : IAnotherActivity
    {
        private readonly ILogger<AnotherActivity> logger;

        public AnotherActivity(ILogger<AnotherActivity> logger)
        {
            this.logger = logger;
        }

        public Task<string> SayHello(string name)
        {
            logger.LogInformation($"Hello {name}");

            return Task.FromResult($"back with {name}");
        }
    }
}