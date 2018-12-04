using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace SME.Models
{
    public class Technology : IEntity
    {
        // private string _id;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string TechnologyId { get; set; }
        public string Name { get; set; }
        [BsonIgnoreIfNull]
        public List<Concept> Concepts { get; set; }

    }
}
