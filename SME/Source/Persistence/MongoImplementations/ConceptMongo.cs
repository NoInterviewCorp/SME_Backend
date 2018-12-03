using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SME.Models;
using SME.Services;

namespace SME.Persistence
{
    public class ConceptMongo : IConceptRepository
    {
        private MongoDbConnection dbConnection;
        public ConceptMongo(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<Concept> AddConceptAsync(Concept concept)
        {
            concept.Name = concept.Name.ToUpper();
            concept._id = Guid.NewGuid().ToString("N");
            var conceptFilter = "{Name:\"" + concept.Name + "\"}";
            var result = await dbConnection.Concepts
                .ReplaceOneAsync(conceptFilter, concept, new UpdateOptions { IsUpsert = true });
            return (result.IsAcknowledged) ? concept : null;
        }

        public async Task<bool> DeleteConceptByNameAsync(string name)
        {
            name = name.ToUpper();
            var removeQuery = await dbConnection.Concepts.DeleteOneAsync(c => c.Name == name);
            if (removeQuery.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Concept>> GetConceptByNameAsync(string name)
        {
            var plans = await dbConnection.Concepts
                .Find("{$text : { $search : \"" + name + "\"} }")
                .ToListAsync();
            return plans;
        }

        public async Task<List<Concept>> GetConceptsAsync()
        {
            var plans = await dbConnection.Concepts
                            .Find(new FilterDefinitionBuilder<Concept>().Empty)
                            .ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }
    }
}