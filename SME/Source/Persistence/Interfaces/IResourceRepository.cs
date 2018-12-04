using SME.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SME.Persistence
{
    public interface IResourceRepository
    {
        // Resource
        Task<Resource> AddResourceAsync(Resource resource);
        Task<List<Resource>> GetResourcesAsync();
        // Task<Resource> GetResourceByLinkAsync(string link);
        Task<List<Resource>> GetResourceByStringAsync(string text);
        Task<Resource> UpdateResourceAsync(Resource resource);
        Task<bool> DeleteResourceAsync (string resourceId);
    }
}