using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

// Document Storage
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Serverless
{
    public static class SignalRTrigger
    {
        // POST http://localhost:7071/api/SignalRTrigger
        /*
        {
            "Id": "1bc8279b-4813-4ae8-a66b-8cd207f2c310",
            "Title": "Do something 2",
            "IsCompleted": true
        }
        */
        
        [FunctionName("SignalRTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] 
            HttpRequest req,
            [SignalR(HubName = "broadcast")]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
             string requestBody = new StreamReader(req.Body).ReadToEnd();

            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("Please pass a payload to broadcast in the request body.");
            }

            var data = JsonConvert.DeserializeObject<MyToDo>(requestBody);

            var documentCollection = Shared.GetDocumentCollection(
                Environment.GetEnvironmentVariable("MongoDbCollection")
            );
            data.Id = Guid.NewGuid().ToString();
            data._Id = MongoDB.Bson.ObjectId.GenerateNewId();

            await documentCollection.InsertOneAsync(data);

            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "notify",
                Arguments = new object[] { data }
            });

            return new CreatedResult($"/api/HttpGet/{data._Id}", data);
        }
    }
}