using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Serverless
{
    public static partial class Functions
    {
        /*
        PUT /todo/1-23-45
        {
            "Title": "Test 2",
            "IsCompleted": true
        }
        */
        [FunctionName(nameof(HttpPutTrigger))]
        [return: ServiceBus(Shared.ServiceBus.DEFAULT_INSTANCE, Connection = Shared.ServiceBus.CONNECTION_NAME)]
        public static async Task<string> HttpPutTrigger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newData = JsonSerializer.Deserialize<MyToDo>(requestBody);

            try
            {
                var collection = Shared.MongoDB<MyToDo>.GetDocumentCollection (
                        Environment.GetEnvironmentVariable ("MongoDbCollection")
                    );
                
                var filter = Builders<MyToDo>.Filter.Eq(nameof(MyToDo.Id), id);
                var update = Builders<MyToDo>.Update.Set(nameof(MyToDo.IsCompleted), newData.IsCompleted);
                collection.UpdateOne(filter, update);

                update = Builders<MyToDo>.Update.Set(nameof(MyToDo.Title), newData.Title);
                collection.UpdateOne(filter, update);

                var todo = await collection
                    .Find(Builders<MyToDo>.Filter.Eq("Id", id))
                    .FirstOrDefaultAsync();

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(todo));
                var data = System.Text.Encoding.UTF8.GetString(bytes);

                return JsonSerializer.Serialize(new DocumentAction {
                    ActionName = Shared.Actions.UPDATE_ACTION,
                    Data = data
                });
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, "Failed MongoDB operation");  

                throw ex;  
            }
        }
    }
}
