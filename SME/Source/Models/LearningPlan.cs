using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace SME.Models
{
    public class LearningPlan
    {
        [BsonId(IdGenerator=typeof(StringObjectIdGenerator))]
        public string LearningPlanId { get; set; }

        public string AuthorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Technology Technology { get; set; }
        public List<Resource> Resources { get; set; }
        public bool HasPublished { get; set; }
    }
}