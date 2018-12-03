using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Resource
    {
        private string _id { get; set; }
        [BsonId]
        public string ResourceId
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResourceLink { get; set; }
        [BsonIgnore]
        public List<Question> Questions { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        // Foreign Keys
        public List<Concept> Concepts { get; set; }
        public List<Technology> Technologies { get; set; }

    }
}