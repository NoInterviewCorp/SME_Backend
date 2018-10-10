using System.Collections.Generic;

namespace SME_Backend.Models
{
    public class Technology
    {
        public int TechnologyId { get; set; }
        public string Name { get; set; }
        public List<Topic> Topics { get; set; }
    }
}