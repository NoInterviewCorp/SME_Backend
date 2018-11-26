using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Concept
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string Name { get; set; }
        [BsonIgnore]
        public List<Technology> Technologies { get; set; }
        [BsonIgnore]
        // foreign key to question
        public List<Question> Questions { get; set; }
        [BsonIgnore]
        // foreign key to resources
        public List<Resource> Resources { get; set; }

    }
}