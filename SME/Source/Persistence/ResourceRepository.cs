using SME.Models;
using SME.Services;
using Neo4jClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace SME.Persistence
{
    public class ResourceRepository : IResourceRepository
    {
        private GraphClient graph;
        public ResourceRepository(GraphDbConnection graph)
        {
            this.graph = graph.Client;
        }
        // Resource
        public async Task<Resource> AddResourceAsync(Resource resource)
        {
            // Add an unique id to resource
            resource.ResourceId = Guid.NewGuid().ToString("N");
            // Linking technologies between resource and each of its concepts
            List<Concept> concepts = new List<Concept>();
            List<Technology> technologies = new List<Technology>();
            if (resource.Concepts != null)
            {
                concepts = new List<Concept>(resource.Concepts);
                resource.Concepts = null;
            }
            else{
                return null;
            }
            if (resource.Technologies != null)
            {
                technologies = new List<Technology>(resource.Technologies);
                resource.Technologies = null;
            }
            else{
                return null;
            }
            // queries
            // query to create a resource
            var matchResource = graph.Cypher
                .Match("(r:Resource)")
                .Where((Resource r) => r.ResourceLink == resource.ResourceLink)
                .Return(r => r.As<Resource>())
                .Results;
            var list = new List<Resource>(matchResource);
            if (list.Count > 0)
            {
                return null;
            }
            else
            {
                var resQuery = new List<Resource>(
                    await graph.Cypher
                        .Create("(res:Resource {resource})")
                        .WithParams(new
                        {
                            resource
                        })
                        .Return(res => res.As<Resource>())
                        .ResultsAsync
                    )[0];
                foreach (Concept concept in concepts)
                {
                    // Converting resource/technology/concept names to lower case
                    // for faster indexed searches
                    concept.Name = concept.Name.ToUpper();
                    await graph.Cypher
                          .Merge("(con:Concept {Name:{conceptName}})")
                          .OnCreate()
                          .Set("con={concept}")
                          .With("con")
                          .Match("(r:Resource)")
                          .Where((Resource r) => r.ResourceId == resource.ResourceId)
                          .Merge("(r)-[:EXPLAINS]->(con)")
                          .WithParams(new
                          {
                              conceptName = concept.Name,
                              concept
                          })
                     .ExecuteWithoutResultsAsync();
                    foreach (Technology technology in technologies)
                    {
                        // Converting resource/technology/concept names to lower case
                        // for faster indexed searches
                        technology.Name = technology.Name.ToUpper();
                        if (technology.TechnologyId == null)
                        {
                            technology.TechnologyId = Guid.NewGuid().ToString("N");
                        }
                        await graph.Cypher
                               .Merge("(t:Technology {Name:{techName}})")
                               .OnCreate()
                               .Set("t={technology}")
                               .With("t")
                               .Match("(con:Concept)")
                               .Where((Concept con) => con.Name == concept.Name)
                               //    .With("con")
                               .Merge("(con)-[:BELONGS_TO]->(t)")
                               .WithParams(new
                               {
                                   techName = technology.Name,
                                   technology
                               })
                               .ExecuteWithoutResultsAsync();
                    }
                }
                return resQuery;
            }
        }

        public async Task<List<Resource>> GetResourcesAsync()
        {
            var results = new List<Resource>(
                await graph.Cypher
                    .Match("(r:Resource)")
                    .Return(r => r.As<Resource>())
                    .ResultsAsync
            );
            foreach (Resource resource in results)
            {
                resource.Concepts = new List<Concept>(
                        await graph.Cypher
                    .Match("(r:Resource)-[:EXPLAINS]->(c:Concept)")
                    .Where((Resource r) => r.ResourceId == resource.ResourceId)
                    .ReturnDistinct(c => c.As<Concept>())
                    .ResultsAsync
                );
                resource.Technologies = new List<Technology>(
                        await graph.Cypher
                    .Match("(r:Resource)-[:EXPLAINS]->(:Concept)-[:BELONGS_TO]->(t:Technology)")
                    .Where((Resource r) => r.ResourceId == resource.ResourceId)
                    .ReturnDistinct(t => t.As<Technology>())
                    .ResultsAsync
                );
            }
            Console.WriteLine("Result is " + results[0].ResourceLink);
            // return new List(results);
            return results;
        }
        // TODO: Add search by link
        public async Task<List<Resource>> GetResourceByStringAsync(string text)
        {
            text = text.ToUpper();
            var query = graph.Cypher
                    .Match("(r:Resource)-[:EXPLAINS]->(:Concept)-[:BELONGS_TO]->(t:Technology)")
                    .Where("t.Name CONTAINS {text}")
                    .WithParams(new
                    {
                        text
                    })
                    .ReturnDistinct(r => r.As<Resource>());
            Console.WriteLine("Query is " + query.Query.QueryText);
            var results = new List<Resource>(
                await query
                    .ResultsAsync
            );
            foreach (Resource resource in results)
            {
                resource.Concepts = new List<Concept>(
                        await graph.Cypher
                    .Match("(r:Resource)-[:EXPLAINS]->(c:Concept)")
                    .Where((Resource r) => r.ResourceId == resource.ResourceId)
                    .ReturnDistinct(c => c.As<Concept>())
                    .ResultsAsync
                );
                resource.Technologies = new List<Technology>(
                        await graph.Cypher
                    .Match("(r:Resource)-[:EXPLAINS]->(:Concept)-[:BELONGS_TO]->(t:Technology)")
                    .Where((Resource r) => r.ResourceId == resource.ResourceId)
                    .ReturnDistinct(t => t.As<Technology>())
                    .ResultsAsync
                );
            }
            return results;
        }
        // public Task<List<Resource>> GetResourceByTechnologyAsync(string technology)
        // {
        //     return null;
        // }

        // TODO: Add the ability to break relationships when a concept or technology
        // is deleted from the resource
        // TODO: Throw error if no concepts & technologies are passed 
        // as they're required
        public async Task<Resource> UpdateResourceAsync(Resource resource)
        {
            // Linking technologies between resource and each of its concepts
            List<Concept> concepts = new List<Concept>();
            List<Technology> technologies = new List<Technology>();
            if (resource.Concepts != null)
            {
                concepts = new List<Concept>(resource.Concepts);
                resource.Concepts = null;
            }
            if (resource.Technologies != null)
            {
                technologies = new List<Technology>(resource.Technologies);
                resource.Technologies = null;
            }
            var result = new List<Resource>(
                await graph.Cypher
                .Match("(r:Resource)")
                .Where((Resource r) => r.ResourceId == resource.ResourceId)
                .Set("r = {resource}")
                .WithParam("resource", resource)
                .Return(r => r.As<Resource>())
                .ResultsAsync
            )[0];
            foreach (Concept concept in concepts)
            {
                // Converting resource/technology/concept names to lower case
                // for faster indexed searches
                concept.Name = concept.Name.ToUpper();
                await graph.Cypher
                      .Merge("(con:Concept {Name:{conceptName}})")
                      .OnCreate()
                      .Set("con={concept}")
                      .With("con")
                      .Match("(r:Resource)")
                      .Where((Resource r) => r.ResourceId == resource.ResourceId)
                      .Merge("(r)-[:EXPLAINS]->(con)")
                      .WithParams(new
                      {
                          conceptName = concept.Name,
                          concept
                      })
                 .ExecuteWithoutResultsAsync();
                foreach (Technology technology in technologies)
                {
                    // Converting resource/technology/concept names to lower case
                    // for faster indexed searches
                    technology.Name = technology.Name.ToUpper();
                    if (technology.TechnologyId == null)
                    {
                        technology.TechnologyId = Guid.NewGuid().ToString("N");
                    }
                    await graph.Cypher
                           .Merge("(t:Technology {Name:{techName}})")
                           .OnCreate()
                           .Set("t={technology}")
                           .With("t")
                           .Match("(con:Concept)")
                           .Where((Concept con) => con.Name == concept.Name)
                           .Merge("(con)-[:BELONGS_TO]->(t)")
                           .WithParams(new
                           {
                               techName = technology.Name,
                               technology
                           })
                           .ExecuteWithoutResultsAsync();
                }
            }
            return result;
        }

        public async Task DeleteResourceAsync(string resourceId)
        {
            // return await graph.Cypher
            //     .Match("(r:Resource)")
            //     .Where((Resource r)=>r.ResourceId == resourceId)
            //     .Delete("r")
            //     .ExecuteWithoutResultsAsync();
            await graph.Cypher
                .OptionalMatch("(r:Resource)-[relation]->()")
                .Where((Resource r) => r.ResourceId == resourceId)
                .Delete("r, relation")
                .ExecuteWithoutResultsAsync();
        }
    }
}