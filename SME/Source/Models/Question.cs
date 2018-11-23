using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace SME.Models
{
    public class Question
    {

        public string QuestionId { get; set; }
        public string ProblemStatement { get; set; }
        public List<Option> Options { get; set; }
        public string CorrectOptionId { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        [BsonIgnore]
        public Resource Resource { get; set; }
        [BsonIgnore]
        public Technology Technology { get; set; }
        [BsonIgnore]
        public List<Concept> Concepts { get; set; }
    }
}