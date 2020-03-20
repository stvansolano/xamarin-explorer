using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Serverless
{
	public class MyToDo
	{
        [BsonId]
        [JsonProperty("_Id")]
        public MongoDB.Bson.ObjectId _Id { get; set; }

        [JsonProperty]
        public string Id { get; set; }


        [BsonElement(nameof(Title))]
        public string Title { get; set; }

        [BsonElement(nameof(IsCompleted))]
        public bool IsCompleted { get; set; }
    }
}
