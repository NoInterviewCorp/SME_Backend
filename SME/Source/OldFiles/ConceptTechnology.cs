using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class ConceptTechnology{
        public string ConceptId { get; set; }
        [JsonIgnore]
        public Concept Concept { get; set; }
        public string TechnologyId { get; set; }
        [JsonIgnore]
        public Technology Technology { get; set; }
    }
}
