using System;
using System.Security.Authentication;
using MongoDB.Driver;

namespace Serverless 
{
    public static partial class Shared 
    {
        public static IMongoCollection<object> GetDocumentCollection (string collectionName)
        {
            var connection = Environment.GetEnvironmentVariable ("MongoDbConnection");
            var databaseName = Environment.GetEnvironmentVariable ("MongoDbDatabase");

            MongoClientSettings settings = MongoClientSettings.FromUrl (
                new MongoUrl (connection)
            );

            settings.SslSettings = new SslSettings () { EnabledSslProtocols = SslProtocols.Tls12 };

            var mongoClient = new MongoClient (settings);
            var client = new MongoClient (connection);
            var database = client.GetDatabase (databaseName);

            IMongoCollection<object> collection = database.GetCollection<object> (collectionName);

            return collection;
        }
    }
}