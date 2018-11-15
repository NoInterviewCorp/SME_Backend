using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class Question
    {
        [Key]
        public string QuestionId { get; set; }
        [Required]
        public string ProblemStatement { get; set; }
        [Required]
        public List<Option> Options { get; set; }
        [Required]
        public BloomTaxonomy BloomLevel { get; set; }
        public bool HasPublished { get; set; }
        public Resource Resource { get; set; }
        public Technology Technology { get; set; }
        public List<Concept> Concepts { get; set; }
    }
}