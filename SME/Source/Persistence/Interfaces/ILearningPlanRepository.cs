using SME.Models;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface ILearningPlanRepository
    {
        // Learning Plan
        LearningPlan AddLearningPlan(LearningPlan learningPlan);
        List<LearningPlan> GetLearningPlans();
        LearningPlan GetLearningPlanById(string learningPlanId);
        List<LearningPlan> GetLearningPlansByUserName(string userName);
        List<LearningPlan> GetLearningPlansByTechnology(string technology);
        LearningPlan UpdateLearningPlan(LearningPlan learningPlan);
    }
}