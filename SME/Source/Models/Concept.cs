using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Concept
    {
        [BsonId]
        public ObjectId ConceptId { get; set; }
        public string Name { get; set; }
        public List<Technology> Technologies { get; set; }
        // foreign key to question
        public List<Question> Questions { get; set; }
        // foreign key to resources
        public List<Resource> Resources { get; set; }

    }
}