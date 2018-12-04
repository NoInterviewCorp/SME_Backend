using System.Linq;
using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace SME.Models
{
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string QuestionId
        {
            get;
            set;
        }

        public string ProblemStatement { get; set; }
        public List<Option> Options { get; set; }
        public Option CorrectOption 
        { 
            get 
            { 
                return Options.Where(o => o.IsCorrect == true).FirstOrDefault();
            }
        }
        
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        
        [BsonIgnore]
        public Technology Technology { get; set; }
        
        [BsonIgnore]
        public List<Concept> Concepts { get; set; }
    }
}