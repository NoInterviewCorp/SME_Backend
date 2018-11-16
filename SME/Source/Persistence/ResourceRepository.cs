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
            if (resource.Technologies != null)
            {
                technologies = new List<Technology>(resource.Technologies);
                resource.Technologies = null;
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
                await graph.Cypher
                    .Create("(res:Resource {resource})")
                    .WithParams(new
                    {
                        resource
                    })
                    .Return(res => res.As<Resource>())
                    .ResultsAsync;
                foreach (Concept concept in concepts)
                {
                    await graph.Cypher
                          .Merge("(con:Concept {Name:{conceptName}})")
                          .OnCreate()
                          .Set("con={concept}")
                          .With("con")
                          .Match("(r:Resource)")
                          .Where((Resource r) => r.ResourceId == resource.ResourceId)
                          .Create("(r)-[:EXPLAINS]->(con)")
                          .WithParams(new
                          {
                              conceptName = concept.Name,
                              concept
                          })
                     .ExecuteWithoutResultsAsync();
                    foreach (Technology technology in technologies)
                    {
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
                               .Create("(con)-[:BELONGS_TO]->(t)")
                               .WithParams(new
                               {
                                   techName = technology.Name,
                                   technology
                               })
                               .ExecuteWithoutResultsAsync();
                    }
                }
                return new List<Resource>(resQuery)[0];
            }
        }



        // public Task<List<Resource>> GetResourcesAsync()
        // {
        //     return null;
        // }
        // public Task<Resource> GetResourceByLinkAsync(string link)
        // {
        //     return null;
        // }
        // public Task<List<Resource>> GetResourceByTechnologyAsync(string technology)
        // {
        //     return null;
        // }
        // public Task<Resource> UpdateResourceAsync(Resource resource)
        // {
        //     return null;
        // }
    }
}