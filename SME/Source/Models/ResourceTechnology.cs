using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ResourceTechnology{
        public string ResourceId { get; set; }
        public Resource Resource { get; set; }
        public string TechnologyId { get; set; }
        public Technology Technology { get; set; }
    }
}