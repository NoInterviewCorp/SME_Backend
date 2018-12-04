using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class ConceptQuestion
    {
        public string ConceptId { get; set; }
        [JsonIgnore]
        public Concept Concept { get; set; }
        public string QuestionId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }
    }
}
