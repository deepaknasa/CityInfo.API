using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Extensions
{
    public static class CityInfoUtilExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                return Enumerable.Empty<T>();
            }
            return sequence;
        }
    }
}
