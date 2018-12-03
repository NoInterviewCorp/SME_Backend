using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SME.Models
{
    public class Technology
    {
        [BsonIgnoreIfDefault][JsonIgnore]
        public ObjectId _id { get; set; }
        public string Name { get; set; }

    }
}