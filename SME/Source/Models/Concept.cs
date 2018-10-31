namespace SME.Models{
    public class Concept{
        [Key]
        public string ConceptId { get; set; }
        [Required]
        public string Name { get; set; }
        // foreign key to topic
        public List<Topic> Topics { get; set;}
        // foreign key to question
        public List<Question> Questions { get; set; }
        // foreign key to resources
        public List<Resource> Resources { get; set; }

    }
}