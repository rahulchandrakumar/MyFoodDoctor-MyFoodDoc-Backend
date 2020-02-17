using System.Collections.Generic;
using System.Linq;

namespace MyFoodDoc.Core
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> Concatenate<T>(params IEnumerable<T>[] lists)
        {
            return lists.SelectMany(x => x);
        }
    }
}
