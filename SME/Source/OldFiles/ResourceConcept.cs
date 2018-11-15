using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class ResourceConcept
    {
        public string ResourceId { get; set; }
        [JsonIgnore]
        public Resource Resource { get; set; }
        public string ConceptId { get; set; }
        [JsonIgnore]
        public Concept Concept { get; set; }
    }
}