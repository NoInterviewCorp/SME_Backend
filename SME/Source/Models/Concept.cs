using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Concept
    {
        private string _id ;
        [BsonId,BsonRepresentation(BsonType.ObjectId)]
        public string ConceptId
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name { get; set; }

    }
}