using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SME.Models;
namespace SME.Services
{
    public class MongoDbConnection
    {
        private readonly IMongoDatabase _db;
        private readonly MongoClient client;
        public MongoDbConnection(IOptions<MongoSettings> options)
        {
            this.client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }
        public IMongoCollection<Resource> Resources => _db.GetCollection<Resource>("Resources");
        public IMongoCollection<LearningPlan> LearningPlans => _db.GetCollection<LearningPlan>("LearningPlans");
        public IMongoCollection<Question> Questions => _db.GetCollection<Question>("Questions");
        public IMongoCollection<Concept> Concepts => _db.GetCollection<Concept>("Concepts");
        public IMongoCollection<Technology> Technologies => _db.GetCollection<Technology>("Technologies");
    }
}