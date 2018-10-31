using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SME.Models
{
    public class Option
    {
        [Key]
        public string OptionId { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public string QuestionId { get; set; }
    }
}