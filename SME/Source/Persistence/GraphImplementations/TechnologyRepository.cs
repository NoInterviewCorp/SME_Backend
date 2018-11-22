using SME.Models;
using SME.Services;
using Neo4jClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;

namespace SME.Persistence
{
    public class TechnologyRepository : ITechnologyRepository
    {
        private GraphClient graph;
        public TechnologyRepository(GraphDbConnection graph)
        {
            this.graph = graph.Client;
        }

        public async Task<List<Technology>> GetAllTechnologiesAsync()
        {
            return new List<Technology>(
                       await graph.Cypher
                   .Match("(r:Resource)")
                   .ReturnDistinct(t => t.As<Technology>())
                   .ResultsAsync
            );
        }
        public async Task<Technology> AddTechnologyAsync(Technology technology)
        {
            technology.TechnologyId = Guid.NewGuid().ToString("N");
            technology.Name = technology.Name.ToUpper();
            var query = new List<Technology>(
                await graph.Cypher
                .Merge("(t:Technology { TechnologyId: {techName} })")
                .OnCreate()
                .Set("t={{technology}}")
                .WithParams(new
                {
                    techName = technology.Name,
                    technology
                })
                .Return(t => t.As<Technology>())
                .ResultsAsync
            );
            return (query.Count == 0) ? null : query[0];
        }
        public async Task<Technology> GetTechnologyByNameAsync(string name)
        {
            name = name.ToUpper();
            var results = new List<Technology>(
                await graph.Cypher
                    .Match("(t:Technology)")
                    .Where((Technology t) => t.Name == name)
                    .Return(t => t.As<Technology>())
                    .ResultsAsync
            );
            return (results.Count == 0) ? null : results[0];
        }
        // public async Task<Technology> UpdateTechnologyAsync(Technology technology)
        // {
        //     return null;
        // }
        public async Task<bool> DeleteTechnologyAsync(string technologyId)
        {
            var result = new List<Technology>(await graph.Cypher
                .Match("(t:Technology {TechnologyId:{id}})")
                .WithParams(new
                {
                    id = technologyId
                })
                .Return(t => t.As<Technology>())
                .ResultsAsync);
            if (result.Count == 0)
            {
                return false;
            }
            await graph.Cypher
                .OptionalMatch("(t:Technology)-[relation]->()")
                .Where((Technology t) => t.TechnologyId == technologyId)
                .Delete("t, relation")
                .ExecuteWithoutResultsAsync();
            return true;
        }
    }
}