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
    public static partial class Functions 
    {
        [FunctionName (nameof(HttpGetTrigger))]
        public static async Task<IActionResult> HttpGetTrigger (
            [HttpTrigger (AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log = null) 
        {
            log.LogInformation ("C# HTTP trigger function processed a request.");

            bool parsed;
            bool displayErrors = bool.TryParse (req.Query["showDetails"].ToString (), out parsed) && parsed;

            try 
            {
                var collection = Shared.MongoDB<MyToDo>.GetDocumentCollection (
                    Environment.GetEnvironmentVariable ("MongoDbCollection")
                );
                var result = await collection.Find(FilterDefinition<MyToDo>.Empty)
                    .ToListAsync<MyToDo>();

                return new OkObjectResult (JsonConvert.SerializeObject (result));
            } 
            catch (System.Exception ex) 
            {
                log.LogInformation ("C# HTTP trigger function processed a request.");

                if (displayErrors) {
                    return new OkObjectResult (ex.Message);
                }
                return new OkObjectResult ("Failed to fetch data");
            }
        }
    }
}
