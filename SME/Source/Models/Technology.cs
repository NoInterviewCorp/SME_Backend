using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Technology
    {
        private string _id;
        
        [BsonId,BsonRepresentation(BsonType.ObjectId)]
        public string TechnologyId
        {
            get { return _id; }
            set { _id = value ?? new ObjectId().ToString(); }
        }
        public string Name { get; set; }

    }
}