using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SME.Models;
using SME.Services;
namespace SME.Persistence
{
    public class TechnologyMongo : ITechnologyRepository
    {
        private MongoDbConnection dbConnection;
        public TechnologyMongo(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<Technology> AddTechnologyAsync(Technology technology)
        {
            technology.Name = technology.Name.ToUpper();
            technology.TechnologyId = Guid.NewGuid().ToString("N");
            var techFilter = "{Name:\"" + technology.Name + "\"}";
            var result = await dbConnection.Technologies
                .ReplaceOneAsync(techFilter, technology, new UpdateOptions { IsUpsert = true });
            return (result.IsAcknowledged) ? technology : null;
        }

        public async Task<bool> DeleteTechnologyAsync(string name)
        {
            // var filter = "{Name:\"" + name + "\"}";
            name = name.ToUpper();
            var removeQuery = await dbConnection.Technologies.DeleteOneAsync(t => t.Name == name);
            if (removeQuery.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Technology>> GetAllTechnologiesAsync()
        {
            var plans = await dbConnection.Technologies
                            .Find(new FilterDefinitionBuilder<Technology>().Empty)
                            .ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<List<Technology>> GetTechnologyByNameAsync(string name)
        {
            var plans = await dbConnection.Technologies
                    .Find("{$text : { $search : \"" + name + "\"} }")
                    .ToListAsync();
            return plans;
        }
    }
}