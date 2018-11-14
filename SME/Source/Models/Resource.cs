using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Resource
    {
        // Entity Attributes
        // [JsonProperty("resourceId")]
        public string ResourceId { get; set; }
        // [JsonProperty("resourceLink")]
        public string ResourceLink { get; set; }
        
        public List<Question> Questions { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        // Foreign Keys
        public List<Concept> Concept { get; set; }
        public List<Technology> Technologies { get; set; }
        public List<Topic> Topics { get; set; }

    }
}