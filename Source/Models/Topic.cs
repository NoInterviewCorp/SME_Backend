using System.Collections.Generic;
namespace Module_SME.Models
{
    public class Topic
    {
        public int TopicId{ get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public int TechnologyId { get; set; }
    }
}