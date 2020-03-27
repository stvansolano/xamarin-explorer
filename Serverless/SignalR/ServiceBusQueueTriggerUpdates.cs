using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Text.Json;
using System.Diagnostics;

namespace Serverless
{
    public static partial class Functions
    {
        [FunctionName(nameof(ServiceBusQueueTriggerUpdates))]
        public static void ServiceBusQueueTriggerUpdates([ServiceBusTrigger(Shared.ServiceBus.DEFAULT_INSTANCE, Connection = Shared.ServiceBus.CONNECTION_NAME)]DocumentAction myQueueItem, 
            [SignalR(HubName = Shared.BROADCAST_HUB)] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            if (myQueueItem == null)
            {
                log.LogWarning("Null queue item");
            }
            log.LogInformation("Triggered service bus message" + JsonSerializer.Serialize(myQueueItem));

            //var todo = JsonSerializer.Deserialize<MyToDo>(myQueueItem.Data);
            //Debug.Assert(todo != null);
            
            signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = Shared.Events.NOTIFY + "_" + myQueueItem.ActionName,
                    Arguments = new[] { myQueueItem }
                });
        }
    }

    public class DocumentAction 
    {
        public string ActionName { get; set; }
        public string Data { get; set; }
    }
}
