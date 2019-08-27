using System;
using System.Collections.Generic;
using System.Text;

namespace Wgaffa.Numbers
{
    public class Interval<T> where T : IComparable<T>
    {
        public EndPoint<T> LowerBound { get; }
        public EndPoint<T> UpperBound { get; }

        public Interval(EndPoint<T> lower, EndPoint<T> upper)
        {
            LowerBound = lower ?? throw new ArgumentNullException(nameof(lower));
            UpperBound = upper ?? throw new ArgumentNullException(nameof(upper));
        }

        public bool Contains(T value)
        {
            return LowerBound.Lower.CompareTo(value) <= 0 && UpperBound.Upper.CompareTo(value) >= 0;
        }
    }
}
