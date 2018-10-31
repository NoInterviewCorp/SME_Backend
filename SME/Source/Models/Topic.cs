using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Topic
    {
        [Key]
        public string TopicId{ get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Resource> Resources { get; set; }
        public string LearningPlanId { get; set; }
    }
}