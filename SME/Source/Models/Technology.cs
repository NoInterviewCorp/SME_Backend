using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Technology
    {
        [Key]
        public int TechnologyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Topic> Topics { get; set; }
    }
}