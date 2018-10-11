namespace SME.Models
{
    public class Option
    {
        [Key]
        public int OptionId { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}