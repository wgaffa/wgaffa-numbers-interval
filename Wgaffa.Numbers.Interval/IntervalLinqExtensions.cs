using System;
using System.Collections.Generic;

namespace Wgaffa.Numbers.Linq
{
    public static class IntervalLinqExtensions
    {
        public static bool Contains<T>(this IEnumerable<Interval<T>> source, T value) where T : IComparable<T>
        {
            foreach (var interval in source)
            {
                if (interval.Contains(value))
                    return true;
            }

            return false;
        }
    }
}
