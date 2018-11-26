using System.Collections.Generic;
using System.Threading.Tasks;
using SME.Models;
using SME.Services;
using MongoDB.Driver;
using System;

namespace SME.Persistence
{
    public class LearningPlanMongo : ILearningPlanRepository
    {
        private MongoDbConnection dbConnection;
        public LearningPlanMongo(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<LearningPlan> AddLearningPlanAsync(LearningPlan learningPlan)
        {
            // Adding LearningPlan to its collections
            var filter = "{Name:\"" + learningPlan.Name + "\",AuthorId:\"" + learningPlan.AuthorId + "\"}";
            var options = new UpdateOptions { IsUpsert = true };

            // Checking if a particular user had already added a learningplan with the same name
            var plan = await dbConnection.LearningPlans.Find(filter).Limit(1).SingleOrDefaultAsync();
            // We do not allow two leaning plans with the same name submitted by the same user
            // to avoid ambiguity
            if (plan != null)
            {
                return null;
            }

            // Delegating the job of upserting to a helper function aynchronously
            learningPlan = await UpsertLearningPlanAsync(learningPlan);

            // finally updating this learning plan in it's own collection
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

        public async Task<LearningPlan> UpdateLearningPlanAsync(LearningPlan learningPlan)
        {
            // Check if this learning plan exist
            // if not then return null 
            // cannot update something that doesn't exist
            if (learningPlan.LearningPlanId == null)
            {
                return null;
            }

            // Delegating the job of upserting other entites to a function
            learningPlan = await UpsertLearningPlanAsync(learningPlan);

            // finally updating this learning plan in it's own collection
            var filter = new FilterDefinitionBuilder<LearningPlan>().Eq("LearningPlanId", learningPlan.LearningPlanId);
            var plan = await dbConnection.LearningPlans.ReplaceOneAsync(
                filter: filter,
                options: new UpdateOptions { IsUpsert = true },
                replacement: learningPlan
            );
            return (plan.IsAcknowledged) ? learningPlan : null;
        }

        public async Task<bool> DeleteLearningPlanAsync(string learningPlanId){
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

        private async Task<LearningPlan> UpsertLearningPlanAsync(LearningPlan learningPlan)
        {
            // preparing bulkwrite containers for each entity
            var resourceModels = new List<WriteModel<Resource>>();
            var questionModels = new List<WriteModel<Question>>();
            var conceptModels = new List<WriteModel<Concept>>();
            var technologyModels = new List<WriteModel<Technology>>();

            // Updating technology field used here in its own collection
            var techFilter = "{Name:\"" + learningPlan.Technology.Name + "\"}";
            var techUpsertQuery = new ReplaceOneModel<Technology>(techFilter, learningPlan.Technology) { IsUpsert = true };
            technologyModels.Add(techUpsertQuery);

            // Updating subsequent entities used inside this learning plan

            foreach (Resource resource in learningPlan.Resources)
            {
                // Allocate id if doesn't exist
                // means this resource is newly added
                // QUESTION: How do we know frontend sent an already existing resource without an id?
                if (resource.ResourceId == null)
                {
                    resource.ResourceId = Guid.NewGuid().ToString("N");
                    resource._Id = new MongoDB.Bson.ObjectId(resource.ResourceId);
                }
                
                var resFilter = "{ResourceId:\"" + resource.ResourceId + "\"}";
                var upsertQuery = new ReplaceOneModel<Resource>(resFilter, resource) { IsUpsert = true };

                // If questions aren't provided initialise to an empty array to avoid errors
                if (resource.Questions == null)
                {
                    resource.Questions = new List<Question>();
                }

                // Upsert questions to it's collection
                foreach (Question question in resource.Questions)
                {
                    if (question.QuestionId == null)
                    {
                        question.QuestionId = Guid.NewGuid().ToString("N");
                    }

                    // Adding concepts mentioned inside a question
                    foreach (Concept concept in question.Concepts)
                    {
                        var conceptFilter = "{Name:\"" + concept.Name + "\"}";
                        var conceptUpsertQuery = new ReplaceOneModel<Concept>(conceptFilter, concept) { IsUpsert = true };
                        conceptModels.Add(conceptUpsertQuery);
                    }

                    // scanning for the correct options
                    string correctId = "";
                    foreach (Option option in question.Options)
                    {
                        option.OptionId = Guid.NewGuid().ToString("N");
                        if (option.IsCorrect)
                        {
                            correctId = option.OptionId;
                        }
                    }
                    question.CorrectOptionId = correctId;
                    var questionFilter = "{QuestionId:\"" + question.QuestionId + "\"}";
                    var questionUpsertQuery = new ReplaceOneModel<Question>(questionFilter, question) { IsUpsert = true };
                    questionModels.Add(questionUpsertQuery);
                }

                // Adding the concepts mentioned in the corresponding resource
                foreach (Concept concept in resource.Concepts)
                {
                    var conceptFilter = "{Name:\"" + concept.Name + "\"}";
                    var conceptUpsertQuery = new ReplaceOneModel<Concept>(conceptFilter, concept) { IsUpsert = true };
                    conceptModels.Add(conceptUpsertQuery);
                }

                // adding all technologies mentioned inside the resource
                foreach (Technology technology in resource.Technologies)
                {
                    var technologyFilter = "{Name:\"" + technology.Name + "\"}";
                    var technologyUpsertQuery = new ReplaceOneModel<Technology>(technologyFilter, technology) { IsUpsert = true };
                    technologyModels.Add(technologyUpsertQuery);
                }

                // adding this resource inside the bulkwrite list
                resourceModels.Add(upsertQuery);
            }

            // Adding all the changes asynchronously into mongo db
            await dbConnection.Technologies.BulkWriteAsync(technologyModels);
            await dbConnection.Concepts.BulkWriteAsync(conceptModels);
            await dbConnection.Resources.BulkWriteAsync(resourceModels);

            // Upsert questions only if they're provided
            if (questionModels.Count > 0)
            {
                await dbConnection.Questions.BulkWriteAsync(questionModels);
            }

            return learningPlan;
        }
    }
}