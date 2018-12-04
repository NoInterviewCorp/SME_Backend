using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SME.Models;
using SME.Services;

namespace SME.Persistence
{
    public class ResourceMongo : IResourceRepository
    {
        private MongoDbConnection dbConnection;
        public ResourceMongo(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<Resource> AddResourceAsync(Resource resource)
        {
            // Allocate id if doesn't exist
            // means this resource is newly added
            // QUESTION: How do we know frontend sent an already existing resource without an id?
            if (resource.ResourceId == null)
            {
                resource.ResourceId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                // resource._id = new MongoDB.Bson.ObjectId(resource.ResourceId);
            }
            resource = await UpsertResourceHelper(resource);
            return resource;
        }

        public async Task<bool> DeleteResourceAsync(string resourceId)
        {
            var removeQuery = await dbConnection.Resources.DeleteOneAsync(r => r.ResourceId == resourceId);
            if (removeQuery.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Resource>> GetResourceByStringAsync(string text)
        {
            var plans = await dbConnection.Resources
                .Find("{$text : { $search : \"" + text + "\"} }")
                .ToListAsync();
            return (plans.Count == 0) ? null : plans;
        }

        public async Task<List<Resource>> GetResourceByAuthorIdAsync(string authorId)
        {
            var filter = new FilterDefinitionBuilder<Resource>().Eq(r=>r.AuthorId, authorId);
            var plans = await dbConnection.Resources.Find(filter).ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<Resource> UpdateResourceAsync(Resource resource)
        {
            if (resource.ResourceId == null)
            {
                return null;
            }
            resource = await UpsertResourceHelper(resource);
            return resource;
        }

        private ReplaceOneModel<Technology> ReplaceOneTechnology(Technology t)
        {
            if (t.TechnologyId == null)
            {
                t.TechnologyId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            }
            var techFilter = "{Name:\"" + t.Name + "\"}";
            return new ReplaceOneModel<Technology>(techFilter, t) { IsUpsert = true };
        }
        private async Task<Resource> UpsertResourceHelper(Resource resource)
        {
            // preparing bulkwrite containers for each entity
            var conceptModels = new List<WriteModel<Concept>>();
            var questionModels = new List<WriteModel<Question>>();
            var technologyModels = new List<WriteModel<Technology>>();

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
                    question.QuestionId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
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
                    option.OptionId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                    if (option.IsCorrect)
                    {
                        correctId = option.OptionId;
                    }
                }
                // question.CorrectOptionId = correctId;
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
            foreach (Technology t in resource.Technologies)
            {
                if (t.TechnologyId == null)
                {
                    t.TechnologyId = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                }
                var techFilter = "{Name:\"" + t.Name + "\"}";
                var technologyUpsertQuery = new ReplaceOneModel<Technology>(techFilter, t) { IsUpsert = true };
                technologyModels.Add(technologyUpsertQuery);
            }


            var resFilter = " { ResourceId:\"" + resource.ResourceId + "\"}";
            var upsertQuery = await dbConnection.Resources.ReplaceOneAsync(resFilter, resource, new UpdateOptions { IsUpsert = true });

            var writeConc = dbConnection.Concepts.BulkWriteAsync(conceptModels);

            // if (technologyModels.Count > 0)
            // {
            // }

            var writeTech = dbConnection.Technologies.BulkWriteAsync(technologyModels);
            if (questionModels.Count > 0)
            {
                await dbConnection.Questions.BulkWriteAsync(questionModels);
            }
            await writeConc;
            await writeTech;
            return resource;
        }
    }
}