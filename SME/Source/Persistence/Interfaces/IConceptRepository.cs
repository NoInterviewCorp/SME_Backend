using SME.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SME.Persistence
{
    public interface IConceptRepository
    {
        // Concept
        Task<Concept> AddConceptAsync(Concept concept);
        Task<List<Concept>> GetConceptsAsync();
        Task<Concept> GetConceptByNameAsync(string name);
        // Task<Concept> UpdateConceptAsync(Concept concept);
        Task<bool> DeleteConceptByNameAsync(string name);
    }
}