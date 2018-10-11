using SME.Models;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface IDatabaseRepository
    {
        List<Technology> GetAllTechnologies();
        List<Topic> GetAllTopicsInATechnology();
        
    }
}