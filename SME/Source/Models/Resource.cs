namespace SME.Models
{
    public class Resource
    {
        [Key]
        public string ResourceId { get; set; }
        [Required]
        public string ResourceLink { get; set; }
        [Required]
        public List<Concept> Concepts { get; set; }
        public List<Technology> Technologies { get; set; }
        public List<Question> Questions { get; set; }
        [Required]
        public BloomTaxonomy BloomLevel { get; set; }
        public string TopicId { get; set; }

    }
}