using System.Collections.Generic;
using SME.Models;

namespace SME.Services
{
    public class ConceptComparer : IEqualityComparer<Concept>
    {
        public bool Equals(Concept x, Concept y)
        {
            if (x == null && y == null)
                return true;
            else if (y == null | x == null)
                return false;
            else if (y.Name.ToUpper() == x.Name.ToUpper())
                return true;
            else
                return false;
        }
        public int GetHashCode(Concept obj)
        {
            return (obj == null) ? 0 : 0;
        }
    }
}