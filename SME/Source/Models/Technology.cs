using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SME.Models
{
    public class Technology
    {
        private string _id;
        [BsonRepresentation(BsonType.ObjectId)]
        public string TechnologyId
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name { get; set; }

    }
}