using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ResourceTechnology{
        [Key]
        public string ResourceId { get; set; }
        public Resource Resource { get; set; }
        [Key]
        public string TechnologyId { get; set; }
        public Technology Technology { get; set; }
    }
}