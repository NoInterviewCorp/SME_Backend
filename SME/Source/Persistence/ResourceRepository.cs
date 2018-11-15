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
                var resQuery = await graph.Cypher
                    .Create("(res:Resource {resource})")
                    // .ForEach("concept in {lConcepts} | MERGE (c:Concept {Name:concept.Name}) ON CREATE c SET c = concept CREATE (res) -[:EXPLAINS]->(c)")
                    .WithParams(new
                    {
                        // lConcepts = concepts,
                        resource
                    })
                    .Return(res => res.As<Resource>())
                    .ResultsAsync;
                Neo4jClient.Cypher.ICypherFluentQuery conceptQuery = graph.Cypher;
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
                            // id = resource.ResourceId,
                            conceptName = concept.Name,
                            concept
                        })
                        .ExecuteWithoutResultsAsync();
                }
                // resQuery= await resQuery.Return(res=>res.As<Resource>()).ExecuteWithoutResultsAsync();
                // await conceptQuery.ExecuteWithoutResultsAsync();
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