using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Technology
    {
        [BsonId]
        public ObjectId TechnologyId { get; set; }
        public string Name { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Concept> Concepts { get; set; }
        public List<LearningPlan> LearningPlans { get; set; }
        public List<Question> Questions { get; set; }

    }
}