using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace SME.Models
{
    public class Concept
    {
        private string _id ;
        
        [BsonId(IdGenerator=typeof(StringObjectIdGenerator)), BsonRepresentation(BsonType.ObjectId)]
        public string ConceptId
        {
            get;
            set;
        }
        public string Name { get; set; }

    }
}