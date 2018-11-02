using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Question
    {
        [Key]
        public strings QuestionId { get; set; }
        [Required]
        public string ProblemStatement { get; set; }
        [Required]
        public List<Option> Options { get; set; }
        [Required]
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        public Resource Resource { get; set; }
        public string ResourceId { get; set; }
        public List<ConceptQuestion> ConceptQuestions { get; set; }
    }
}