using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace SME.Models
{
    public class Concept
    {
        private string _id ;
        
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string ConceptId
        {
            get;
            set;
        } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }

    }
}