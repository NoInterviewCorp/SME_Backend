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
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteResourceAsync(string resourceId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Resource>> GetResourceByStringAsync(string text)
        {
            var plans = await dbConnection.Resources
                .Find("{$text : { $search : \"" + text + "\"} }")
                .ToListAsync();
            return (plans.Count == 0) ? null : plans;
        }

        public async Task<List<Resource>> GetResourcesAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Resource> UpdateResourceAsync(Resource resource)
        {
            throw new System.NotImplementedException();
        }

    }
}