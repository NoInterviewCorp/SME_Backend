using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace SME.Models
{
    public class Option
    {
        public string OptionId { get; set; }
        public string Content { get; set; }
        [BsonIgnore]
        public bool IsCorrect { get; set; }
    }
}