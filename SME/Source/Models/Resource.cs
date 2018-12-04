using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SME.Models
{
    public class Resource : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string ResourceId
        {
            get;
            set;
        }
        public string AuthorId { get; set; }
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