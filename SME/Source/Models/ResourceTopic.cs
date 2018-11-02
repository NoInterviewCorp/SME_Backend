using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ResourceTopic
    {
        [Key]
        public string ResourceId { get; set; }
        public Resource Resource { get; set; }
        [Key]
        public string TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}