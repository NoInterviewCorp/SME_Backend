using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace SME.Models
{
    public class Technology
    {
        private string _id;
        
        [BsonId(IdGenerator=typeof(StringObjectIdGenerator)), BsonRepresentation(BsonType.ObjectId)]
        public string TechnologyId
        {
            get;
            set;
        } = new ObjectId().ToString();
        public string Name { get; set; }

    }
}