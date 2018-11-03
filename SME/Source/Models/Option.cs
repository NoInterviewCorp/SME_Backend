using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
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
        [JsonIgnore]
        public Question Question { get; set; }
    }
}