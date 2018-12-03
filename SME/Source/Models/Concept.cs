using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
        } = new ObjectId().ToString();
        public string Name { get; set; }

    }
}