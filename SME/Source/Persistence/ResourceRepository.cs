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
            resource.ResourceId = Guid.NewGuid().ToString("N");
            var result = await graph.Cypher
                .Merge("(resourceNode:Resource {ResourceLink : {link} }) ")
                .OnCreate()
                .Set("resourceNode = {resource}")
                .WithParams(new
                {
                    link = resource.ResourceLink,
                    resource
                })
                .Return(resourceNode => resourceNode.As<Resource>())
                .ResultsAsync;
            List<Resource> resultList = new List<Resource>(result);
            if (resultList.Count > 0)
            {
                // Console.WriteLine(resultList.Count);
                return resultList[0];
            }
            return null;
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