using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace SME.Models
{
    public class LearningPlan
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string LearningPlanId { get; set; }
        public string AuthorId { get; set; }
        public string Name { get; set; }
        public Technology Technology { get; set; }
        public List<Resource> Resources { get; set; }
        public bool HasPublished { get; set; }
    }
}