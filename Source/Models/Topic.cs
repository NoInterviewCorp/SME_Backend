using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Topic
    {
        [Key]
        public int TopicId{ get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Question> Questions { get; set; }
        public int TechnologyId { get; set; }
    }
}