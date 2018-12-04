using SME.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace SME.Persistence
{
    public interface ILearningPlanRepository
    {
        // Learning Plan
        Task<ReplaceOneResult> AddLearningPlanAsync(LearningPlan learningPlan);
        Task<List<LearningPlan>> GetLearningPlansAsync();
        Task<LearningPlan> GetLearningPlanByIdAsync(string learningPlanId);
        Task<List<LearningPlan>> GetLearningPlansByUserNameAsync(string userName);
        Task<List<LearningPlan>> GetLearningPlansByTechnologyAsync(string technology);
        
        Task UpdateLearningPlanAsync(LearningPlan learningPlan);
        Task<bool> DeleteLearningPlanAsync(string learningPlanId);
    }
}