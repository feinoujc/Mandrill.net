using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill.Serialization
{
    static class ListHelper
    {
        public static List<T> SafeToList<T>(this IEnumerable<T> items)
        {
            if (items == null)
                return null;
            return items.ToList();
        }
    }
}
