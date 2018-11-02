using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models{
    public class Concept{
        [Key]
        public string ConceptId { get; set; }
        [Required]
        public string Name { get; set; }
        // foreign key to topic
        public List<ConceptTechnology> ConceptTechnologies { get; set;}
        // foreign key to question
        public List<ConceptQuestion> ConceptQuestions { get; set; }
        // foreign key to resources
        public List<ResourceConcept> ResourceConcepts { get; set; }

    }
}