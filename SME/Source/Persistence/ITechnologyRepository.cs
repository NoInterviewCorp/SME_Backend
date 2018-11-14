using SME.Models;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface ITechnologyRepository
    {
        // Technology
        List<Technology> GetAllTechnologies();
        Technology AddTechnology(Technology technology);
        Technology GetTechnologyByName(string name);
        Technology UpdateTechnology(Technology technology);
    }
}