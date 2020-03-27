using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;

// Document Storage
//using Microsoft.Azure.Documents;
//using System.Collections.Generic;

namespace Serverless
{
    public static partial class Functions
    {
        // POST http://localhost:7071/api/HttpPostTrigger
        /*
        {
            "_t": "MyToDo",
            "Title": "Do something 3",
            "IsCompleted": false
        }
        */
        
        [FunctionName(nameof(HttpPostTrigger))]
        public static async Task<IActionResult> HttpPostTrigger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] 
            HttpRequest req,
            [SignalR(HubName = Shared.BROADCAST_HUB)]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
             string requestBody = new StreamReader(req.Body).ReadToEnd();

            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("Please pass a payload to broadcast in the request body.");
            }

            var data = JsonConvert.DeserializeObject<MyToDo>(requestBody);

            var documentCollection = Shared.MongoDB<MyToDo>.GetDocumentCollection(
                Environment.GetEnvironmentVariable("MongoDbCollection")
            );
            if (data.Id == Guid.Empty.ToString())
            {
                data.Id = Guid.NewGuid().ToString();
            }
            data._Id = MongoDB.Bson.ObjectId.GenerateNewId();
            data.DateCreated = System.DateTime.UtcNow;
            
            await documentCollection.InsertOneAsync(data);

            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = Shared.Events.NOTIFY, 
                //UserId = "Web",
                Arguments = new object[] { data },
                //GroupName = "ToDos"
            });

            return new CreatedResult($"/api/HttpGet/{data._Id}", data);
        }
    }
}