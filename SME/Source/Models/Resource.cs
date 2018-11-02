using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Resource
    {
        // Entity Attributes
        [Key]
        public string ResourceId { get; set; }
        [Required]
        public string ResourceLink { get; set; }
        [Required]
        public List<Question> Questions { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        // Foreign Keys
        [Required]
        public List<ResourceConcept> ResourceConcepts { get; set; }
        public List<ResourceTechnology> ResourceTechnologies { get; set; }
        public List<ResourceTopic> ResourceTopics { get; set; }

    }
}