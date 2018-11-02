using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ResourceConcept
    {
        [Key]
        public string ResourceId { get; set; }
        public Resource Resource { get; set; }
        [Key]
        public string ConceptId { get; set; }
        public Concept Concept { get; set; }
    }
}