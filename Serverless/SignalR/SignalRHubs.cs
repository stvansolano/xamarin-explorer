namespace Serverless
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.WebJobs.Extensions.SignalRService;

    public static class SignalRHubs
    {
        // POST: https://my-signalr-functions.azurewebsites.net/api/hubs/broadcast/negotiate
        
        [FunctionName(nameof(Broadcast))]
        public static async Task<IActionResult> Broadcast(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hubs/" + Shared.BROADCAST_HUB + "/negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = Shared.BROADCAST_HUB)]SignalRConnectionInfo info, 
            ILogger log)
        {
            return await Task.FromResult(
                info != null
                ? (ActionResult)new OkObjectResult(info)
                : new NotFoundObjectResult("Failed to load SignalR Info."));
        }
    }
}