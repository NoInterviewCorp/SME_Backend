using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
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
        [JsonIgnore]
        public LearningPlan LearningPlan { get; set; }
    }
}