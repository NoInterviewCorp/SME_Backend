using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class ResourceTopic
    {
        public string ResourceId { get; set; }
        [JsonIgnore]
        public Resource Resource { get; set; }
        public string TopicId { get; set; }
        [JsonIgnore]
        public Topic Topic { get; set; }
    }
}