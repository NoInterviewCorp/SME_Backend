using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Resource
    {
        [BsonId(IdGenerator=typeof(StringObjectIdGenerator)),BsonRepresentation(BsonType.ObjectId)]
        public string ResourceId
        {
            get; 
            set;
        } = new ObjectId().ToString();
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