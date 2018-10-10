using System.Collections.Generic;
namespace SME.Models
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string ProblemStatement { get; set; }
        public List<Option> Options { get; set; }
        public string ResourceLink { get; set; }
        public BloomTaxonomy BloomLevel { get; set; }
        public int TopicId { get; set; }
    }
}