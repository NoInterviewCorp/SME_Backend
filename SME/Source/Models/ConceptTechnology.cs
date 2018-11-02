using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ConceptTechnology{
        [Key]
        public string ConceptId { get; set; }
        public Concept Concept { get; set; }
        [Key]
        public string TechnologyId { get; set; }
        public Technology Technology { get; set; }
    }
}
