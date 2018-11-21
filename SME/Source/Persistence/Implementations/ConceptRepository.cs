using SME.Models;
using SME.Services;
using Neo4jClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;

namespace SME.Persistence
{
    public class ConceptRepository : IConceptRepository
    {
        private GraphClient graph;
        public ConceptRepository(GraphDbConnection graph)
        {
            this.graph = graph.Client;
        }

        // Concept

        public async Task<Concept> AddConceptAsync(Concept concept)
        {

            // Converting resource/technology/concept names to upper case
            // for faster indexed searches
            concept.Name = concept.Name.ToUpper();
            return new List<Concept>(await graph.Cypher
                  .Merge("(con:Concept { Name: {conceptName} })")
                  .OnCreate()
                  .Set("con={concept}")
                  .WithParams(new
                  {
                      conceptName = concept.Name,
                      concept
                  })
                  .Return(con => con.As<Concept>())
                  .ResultsAsync)[0];
        }
        public async Task<List<Concept>> GetConceptsAsync()
        {
            return new List<Concept>(
                await graph.Cypher
                    .Match("(c:Concept)")
                    .Return(c => c.As<Concept>())
                    .ResultsAsync
            );
        }
        public async Task<Concept> GetConceptByNameAsync(string name)
        {
            name = name.ToUpper();
            var results = new List<Concept>(
                await graph.Cypher
                    .Match("(c:Concept)")
                    .Where((Concept c) => c.Name == name)
                    .Return(c => c.As<Concept>())
                    .ResultsAsync
            );
            return (results.Count == 0) ? null : results[0];
        }
        // public async Task<Concept> UpdateConceptAsync(Concept concept)
        // {
        //     // Converting resource/technology/concept names to upper case
        //     // for faster indexed searches
        //     concept.Name = concept.Name.ToUpper();
        //     return new List<Concept>(await graph.Cypher
        //           .Merge("(con:Concept { Name: {conceptName} })")
        //           .OnMatch()
        //           .Set("con={concept}")
        //           .WithParams(new
        //           {
        //               conceptName = concept.Name,
        //               concept
        //           })
        //           .Return(con => con.As<Concept>())
        //           .ResultsAsync)[0];
        // }

        public async Task<bool> DeleteConceptByNameAsync(string name)
        {
            name = name.ToUpper();
            var result = new List<Concept>(await graph.Cypher
                .Match("(c:Concept {Name:{conceptName}})")
                .WithParams(new{
                    conceptName = name
                })
                .Return(c=>c.As<Concept>())
                .ResultsAsync);
            if(result.Count == 0){
                return false;
            }
            await graph.Cypher
                .OptionalMatch("(c:Concept)-[relation]->()")
                .Where((Concept c) => c.Name == name)
                .Delete("c, relation")
                .ExecuteWithoutResultsAsync();
            return true;
        }
    }
}