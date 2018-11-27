using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Technology
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        [BsonIgnore]
        public List<Resource> Resources { get; set; }
        [BsonIgnore]
        public List<Concept> Concepts { get; set; }
        [BsonIgnore]
        public List<LearningPlan> LearningPlans { get; set; }
        [BsonIgnore]
        public List<Question> Questions { get; set; }

    }
}