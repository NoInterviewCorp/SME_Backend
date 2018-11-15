using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models{
    public class Concept{
        // [Key]
        // public string ConceptId { get; set; }
        [Required,Key]
        public string Name { get; set; }
        // foreign key to topic
        public List<Technology> Technologies { get; set;}
        // foreign key to question
        public List<Question> Questions { get; set; }
        // foreign key to resources
        public List<Resource> Resources { get; set; }

    }
}