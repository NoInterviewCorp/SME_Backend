using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class ResourceTechnology{
        public string ResourceId { get; set; }
        [JsonIgnore]
        public Resource Resource { get; set; }
        public string TechnologyId { get; set; }
        [JsonIgnore]
        public Technology Technology { get; set; }
    }
}