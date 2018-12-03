using SME.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Persistence
{
    public interface ILearningPlanRepository
    {
        // Learning Plan
        Task AddLearningPlanAsync(LearningPlan learningPlan);
        Task<List<LearningPlan>> GetLearningPlansAsync();
        Task<LearningPlan> GetLearningPlanByIdAsync(string learningPlanId);
        Task<List<LearningPlan>> GetLearningPlansByUserNameAsync(string userName);
        Task<List<LearningPlan>> GetLearningPlansByTechnologyAsync(string technology);
        
        Task UpdateLearningPlanAsync(LearningPlan learningPlan);
        Task<bool> DeleteLearningPlanAsync(string learningPlanId);
    }
}