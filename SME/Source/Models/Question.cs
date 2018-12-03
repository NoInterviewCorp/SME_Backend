using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace SME.Models
{
    public class Question
    {
        private string _id { get; set; }
        [BsonId,BsonRepresentation(BsonType.ObjectId)]
        public string QuestionId
        {
            get { return _id; }
            set { _id = value; }
        }
        public string ProblemStatement { get; set; }
        public List<Option> Options { get; set; }
        public string CorrectOptionId { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        [BsonIgnore]
        public Technology Technology { get; set; }
        [BsonIgnore]
        public List<Concept> Concepts { get; set; }
    }
}