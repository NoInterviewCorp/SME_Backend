using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Technology
    {
        [Key]
        public string TechnologyId { get; set; }
        [Required]
        public string Name { get; set; }
        // Many-Many relationships
        public List<ResourceTechnology> ResourceTechnologies { get; set; }
        public List<ConceptTechnology> ConceptTechnologies { get; set; }
        // Foreign Key for One-Many relationship
        public List<LearningPlan> LearningPlans { get; set; }
        public List<Question> Questions { get; set; }

    }
}