using SME.Models;
using System.Linq;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface IConceptRepository
    {
        // Concept
        Concept AddConcept(Concept concept, bool skipFind = false);
        List<Concept> GetConcepts();
        Concept GetConceptByName(string name);
        Concept UpdateConcept(Concept concept);

    }
}