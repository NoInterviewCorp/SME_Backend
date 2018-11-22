using System.Collections.Generic;
using System.Threading.Tasks;
using SME.Models;
using System.Threading.Tasks;
using SME.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

namespace SME.Persistence
{
    public class LearningPlanRepository : ILearningPlanRepository
    {
        private MongoDbConnection dbConnection;
        public LearningPlanRepository(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<LearningPlan> AddLearningPlanAsync(LearningPlan learningPlan)
        {
            // Upserting resources in the resource collection
            var models = new List<WriteModel<Resource>>();
            var questionModels = new List<WriteModel<Question>>();
            foreach (Resource resource in learningPlan.Resources)
            {
                if(resource.ResourceId==null){
                    resource.ResourceId = new ObjectId(Guid.NewGuid().ToString("N"));
                }
                var resFilter = "{_id:" + resource.ResourceId + "}";
                var upsertQuery = new ReplaceOneModel<Resource>(resFilter, resource) { IsUpsert = true };

                foreach (Question question in resource.Questions)
                {
                    if (question.QuestionId == null)
                    {
                        question.QuestionId = new ObjectId(Guid.NewGuid().ToString("N"));
                    }
                    var questionFilter = "{_id:" + question.ProblemStatement + "}";
                    var questionUpsertQuery = new ReplaceOneModel<Question>(questionFilter, question) { IsUpsert = true };
                    questionModels.Add(questionUpsertQuery);
                }
                models.Add(upsertQuery);
            }
            await dbConnection.Questions.BulkWriteAsync(questionModels);
            await dbConnection.Resources.BulkWriteAsync(models);
            // Adding LearningPlan to its collections
            var filter = "{Name:" + learningPlan.Name + ",AuthorId:" + learningPlan.AuthorId + "}";
            var options = new UpdateOptions { IsUpsert = true };
            await dbConnection.LearningPlans.ReplaceOneAsync(filter, learningPlan, options);
            return learningPlan;
        }

        public async Task<LearningPlan> GetLearningPlanByIdAsync(string learningPlanId)
        {
            var filter = new FilterDefinitionBuilder<LearningPlan>().Eq("LearningPlanId", learningPlanId);
            var plan = await dbConnection.LearningPlans.Find(filter).Limit(1).SingleOrDefaultAsync();
            return plan;
        }

        public async Task<List<LearningPlan>> GetLearningPlansAsync()
        {
            var filter = new FilterDefinitionBuilder<LearningPlan>().Empty;
            var plans = await dbConnection.LearningPlans.Find(filter).ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<List<LearningPlan>> GetLearningPlansByTechnologyAsync(string technology)
        {
            var filter = "{ Technology : " + technology + " }";
            var plans = await dbConnection.LearningPlans.Find(filter).ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<List<LearningPlan>> GetLearningPlansByUserNameAsync(string userName)
        {
            var filter = "{ AuthorId : " + userName + " }";
            var plans = await dbConnection.LearningPlans.Find(filter).ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<LearningPlan> UpdateLearningPlanAsync(LearningPlan learningPlan)
        {
            var filter = new FilterDefinitionBuilder<LearningPlan>().Eq("LearningPlanId", learningPlan.LearningPlanId);
            var plan = await dbConnection.LearningPlans.ReplaceOneAsync(
                filter: filter,
                options: new UpdateOptions { IsUpsert = true },
                replacement: learningPlan
            );
            return (plan.IsAcknowledged) ? learningPlan : null;
        }
    }
}