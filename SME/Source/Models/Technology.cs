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
        public List<Resource> Resources { get; set; }
        public List<Concept> Concepts { get; set; }
        public List<LearningPlan> LearningPlans { get; set; }
        public List<Question> Questions { get; set; }

    }
}