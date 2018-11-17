using SME.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SME.Persistence
{
    public interface ITechnologyRepository
    {
        // Technology
        Task<List<Technology>> GetAllTechnologiesAsync();
        Task<Technology> AddTechnologyAsync(Technology technology);
        Task<Technology> GetTechnologyByNameAsync(string name);
        // Task<Technology> UpdateTechnologyAsync(Technology technology);
        Task<bool> DeleteTechnologyAsync(string technologyId);
    }
}