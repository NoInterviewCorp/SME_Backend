using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace SME.Models
{
    public class Question
    {
        [BsonId]
        public ObjectId QuestionId { get; set; }
        public string ProblemStatement { get; set; }
        public List<Option> Options { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        public Resource Resource { get; set; }
        public Technology Technology { get; set; }
        public List<Concept> Concepts { get; set; }
    }
}