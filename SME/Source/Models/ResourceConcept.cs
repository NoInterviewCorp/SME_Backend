using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ResourceConcept
    {
        public string ResourceId { get; set; }
        public Resource Resource { get; set; }
        public string ConceptId { get; set; }
        public Concept Concept { get; set; }
    }
}