using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class ConceptQuestion
    {
        public string ConceptId { get; set; }
        public Concept Concept { get; set; }
        public string QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
