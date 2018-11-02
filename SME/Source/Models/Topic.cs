using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Topic
    {
        [Key]
        public string TopicId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<ResourceTopic> ResourceTopics { get; set; }
        public string LearningPlanId { get; set; }
        public LearningPlan LearningPlan { get; set; }
    }
}