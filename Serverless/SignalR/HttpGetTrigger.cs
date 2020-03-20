using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Serverless
{
    public static class HttpGetTrigger
    {
        [FunctionName("HttpGetTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log = null)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            bool parsed;
            bool displayErrors = bool.TryParse(req.Query["displayErrors"].ToString(), out parsed) && parsed;

//          string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//            dynamic data = JsonConvert.DeserializeObject(requestBody);

            try
            {
                 var connection = Environment.GetEnvironmentVariable("MongoDbConnection");
                var databaseName = Environment.GetEnvironmentVariable("MongoDbDatabase");
                var collectionName = Environment.GetEnvironmentVariable("MongoDbCollection");

                MongoClientSettings settings = MongoClientSettings.FromUrl(
                    new MongoUrl(connection)
                );

                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                var mongoClient = new MongoClient(settings);
                var client = new MongoClient(connection);
                var database = client.GetDatabase(databaseName);

                IMongoCollection<object> collection = database.GetCollection<object>(collectionName);

                var result = await collection.Find(FilterDefinition<object>.Empty)
                                            .ToListAsync<object>();

                return new OkObjectResult(JsonConvert.SerializeObject(result));
            }
            catch (System.Exception ex)
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                if (displayErrors){
                    return new OkObjectResult(ex.Message);
                }
                return new OkObjectResult("Failed to fetch data");
            }
        }
    }
}
