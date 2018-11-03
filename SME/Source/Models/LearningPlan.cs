using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace SME.Models
{
    public class LearningPlan
    {
        public string UserName { get; set; }
        public string LearningPlanId { get; set; }
        public string Name { get; set; }
        public List<Topic> Topics { get; set; }
        public bool HasPublished { get; set; }
        // foreign key to technology
        public string TechnologyId { get; set; }
        [JsonIgnore]
        public Technology Technology { get; set; }

    }
}