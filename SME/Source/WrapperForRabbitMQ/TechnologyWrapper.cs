using System.Collections.Generic;

namespace SME.Models
{
    public class TechnologyWrapper
    {
        public TechnologyWrapper(Technology t)
        {
            Name = t.Name;
        }
        public string Name { get; set; }
    }
}