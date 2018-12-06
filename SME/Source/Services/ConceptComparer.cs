using System.Collections.Generic;
using SME.Models;

namespace SME.Services
{
    public class EntityComparer<T> : IEqualityComparer<T> where T : IEntity 
    {
        public bool Equals(T x, T y)
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
        public int GetHashCode(T obj)
        {
            return (obj == null) ? 0 : 0;
        }
    }
}