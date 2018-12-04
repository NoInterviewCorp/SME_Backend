using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using MongoDB.Driver;
using MongoDB.Bson;

using SME.Models;
using SME.Services;


namespace SME.Persistence
{
    public class LearningPlanMongo : ILearningPlanRepository
    {
        private MongoDbConnection dbConnection;
        public LearningPlanMongo(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<ReplaceOneResult> AddLearningPlanAsync(LearningPlan learningPlan)
        {
            // TODO: Throw 400 BadRequest when the user tries 
            // to update a Learning Plan which doesn't exist
            Console.WriteLine("Checking Learning Plan Id");
            Console.WriteLine(learningPlan.LearningPlanId);

            // Adding LearningPlan to its collections
            var filter = "{Name:\"" + learningPlan.Name + "\",AuthorId:\"" + learningPlan.AuthorId + "\"}";
            var options = new UpdateOptions { IsUpsert = true };

            // Checking if a particular user had already added a learningplan with the same name
            var plan = await dbConnection.LearningPlans.Find(filter).Limit(1).SingleOrDefaultAsync();
            // We do not allow two learning plans with the same name submitted by the same user
            // to avoid ambiguity
            if (plan != null)
            {
                throw new Exception("The same user cannot add two Learning Plans of the same name.");
            }
            // Delegating the job of upserting to a helper function aynchronously
            return await UpsertLearningPlanAsync(learningPlan);
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
            var filter = "{ Technology : \"" + technology + "\" }";
            var plans = await dbConnection.LearningPlans.Find(filter).ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<List<LearningPlan>> GetLearningPlansByUserNameAsync(string userName)
        {
            var filter = "{ AuthorId : \"" + userName + "\" }";
            var plans = await dbConnection.LearningPlans.Find(filter).ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task UpdateLearningPlanAsync(LearningPlan learningPlan)
        {
            // TODO: Throw 400 BadRequest when the user tries 
            // to update a Learning Plan which doesn't exist
            if (learningPlan.LearningPlanId == null)
            {
                throw new Exception("Cannot update a learning plan which doesn't exist. Please provide an ID if you wish to update a LearningPlan");
            }
            // Delegating the job of upserting other entites to a function
            await UpsertLearningPlanAsync(learningPlan);
        }

        public async Task<bool> DeleteLearningPlanAsync(string learningPlanId)
        {
            var removeQuery = await dbConnection.LearningPlans.DeleteOneAsync(l => l.LearningPlanId == learningPlanId);
            if (removeQuery.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private ReplaceOneModel<T> ReplaceOneEntity<T>(T t) where T : IEntity
        {
            var filter = "{Name:\"" + t.Name + "\"}";
            return new ReplaceOneModel<T>(filter, t) { IsUpsert = true };
        }

        private ReplaceOneModel<Question> ReplaceOneQuestion(Question question)
        {
            var questionFilter = "{QuestionId:\"" + question.QuestionId + "\"}";
            return new ReplaceOneModel<Question>(questionFilter, question) { IsUpsert = true };
        }

        private async Task<ReplaceOneResult> UpsertLearningPlanAsync(LearningPlan learningPlan)
        {
            var learningPlanFilter = "{Name:\"" + learningPlan.Name + "\",AuthorId:\"" + learningPlan.AuthorId + "\"}";
            var insertLearningPlan = dbConnection.LearningPlans.ReplaceOneAsync(learningPlanFilter, learningPlan, new UpdateOptions() { IsUpsert = true });

            var resources =
                learningPlan.Resources.Select(ReplaceOneEntity)
                .ToList();

            var bulkWriteResources = resources.Count > 0
                ? dbConnection.Resources.BulkWriteAsync(resources)
                : Task.CompletedTask;

            var technologies =
                learningPlan.Resources.SelectMany(r => r.Technologies)
                .Union(new List<Technology>() { learningPlan.Technology })
                .Select(ReplaceOneEntity)
                .ToList();

            var bulkWriteTechnologies = technologies.Count > 0
                ? dbConnection.Technologies.BulkWriteAsync(technologies)
                : Task.CompletedTask;

            var concepts =
                learningPlan.Resources.SelectMany(r => r.Concepts)
                .Union(learningPlan.Resources.SelectMany(r => r.Questions)
                .SelectMany(q => q.Concepts))
                .Select(ReplaceOneEntity)
                .ToList();

            var bulkWriteConcepts = concepts.Count > 0
                ? dbConnection.Concepts.BulkWriteAsync(concepts)
                : Task.CompletedTask;

            var questions =
                learningPlan.Resources.SelectMany(q => q.Questions)
                .Select(ReplaceOneQuestion)
                .ToList();

            var bulkWriteQuestions = questions.Count > 0
                ? dbConnection.Questions.BulkWriteAsync(questions)
                : Task.CompletedTask;
            await bulkWriteResources;
            await bulkWriteTechnologies;
            await bulkWriteConcepts;
            await bulkWriteQuestions;
            await AddConceptsToTechnologies(learningPlan);
            return await insertLearningPlan;
        }

        private async Task AddConceptsToTechnologies(LearningPlan learningPlan)
        {
            IEqualityComparer<Concept> comparer = new ConceptComparer();
            var conceptsOfTechnology =
                learningPlan.Resources.SelectMany(r => r.Concepts)
                .Union(learningPlan.Resources.SelectMany(r => r.Questions)
                .SelectMany(q => q.Concepts))
                .Distinct(comparer)
                .ToList();
            var technologyName = learningPlan.Technology.Name;
            var filter = Builders<Technology>.Filter.Where(t => t.Name == technologyName);
            var technologyUpdateDefinition = Builders<Technology>.Update
                .PushEach(t => t.Concepts, conceptsOfTechnology);
            await dbConnection.Technologies.FindOneAndUpdateAsync(filter, technologyUpdateDefinition, new FindOneAndUpdateOptions<Technology>() { IsUpsert = true });
        }
    }
}
