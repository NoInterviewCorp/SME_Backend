namespace SME.Models{
    public class LearningPlan{
        public string LearningPlanId { get; set; }
        public List<Topic> Topics { get; set; }
        // foreign key to technology
        public Technology Technology { get; set; }

    }
}