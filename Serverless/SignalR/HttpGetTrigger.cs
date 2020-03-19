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
    public static class ToDoGet
    {
        [FunctionName("ToDoGet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

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

            IMongoCollection<MyToDo> collection = database.GetCollection<MyToDo>(collectionName);

            var result = await collection.Find(FilterDefinition<MyToDo>.Empty)
                                         .ToListAsync<MyToDo>();

            return new OkObjectResult(JsonConvert.SerializeObject(result));
        }
    }
}