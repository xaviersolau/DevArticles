//using DurableFunctionApp1.Business;
//using DurableLib;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.DependencyInjection;

//namespace DurableFunctionApp1.Generated
//{
//    /// <summary>
//    /// Generated
//    /// </summary>
//    public class MyActivitiesFunction
//    {
//        public record SayHelloPayload
//        {
//            public string Name { get; set; }
//            public string Comment { get; set; }
//        }

//        [Function(nameof(IMyActivities.SayHello))]
//        public static Task<string> SayHelloFunction([ActivityTrigger] SayHelloPayload payload, FunctionContext executionContext)
//        {
//            var name = payload.Name;
//            var comment = payload.Comment;

//            var activities = executionContext.InstanceServices.GetRequiredService<IMyActivities>();

//            return activities.SayHello(name, comment);
//        }

//        /// <summary>
//        /// Generated
//        /// </summary>
//        public sealed class Client : ActivityClientBase, IMyActivities
//        {
//            public Task<string> SayHello(string name, string comment)
//            {
//                var payload = new SayHelloPayload
//                {
//                    Name = name,
//                    Comment = comment,
//                };

//                return Context.CallActivityAsync<string>(nameof(IMyActivities.SayHello), payload);
//            }
//        }

//        public sealed class ClientFactory : ActivityFactory<IMyActivities, Client>
//        {
//        }
//    }
//}
