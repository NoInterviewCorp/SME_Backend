using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace SME.Models
{
    public class Concept: IEntity
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string ConceptId
        {
            get;
            set;
        }
        public string Name { get; set; }

    }
}