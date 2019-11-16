using System;
using System.Collections.Generic;
using System.Linq;

namespace Wgaffa.Numbers
{
    public class IntInterval : Interval<int>
    {
        public IntInterval(EndPoint<int> lower, EndPoint<int> upper) : base(lower, upper)
        {
        }

        public IEnumerable<IntInterval> Union(IEnumerable<IntInterval> intervals)
        {
            if (intervals == null)
                throw new ArgumentNullException(nameof(intervals));

            var baseCollection = base.Union(intervals);
            var casted = baseCollection.Cast<IntInterval>();

            return casted;
        }

        public override bool Overlaps(Interval<int> other)
        {
            if (other == null)
                return false;

            var continuousLeft = (Lower.Inclusive && other.Upper.Inclusive) && Math.Abs(Lower.Value - other.Upper.Value) <= 1;
            var continuousRight = (Upper.Inclusive && other.Lower.Inclusive) && Math.Abs(Upper.Value - other.Lower.Value) <= 1;

            if (continuousLeft || continuousRight)
                return true;

            return Lower.IsBefore(other.Upper) && other.Lower.IsBefore(Upper);
        }

        protected override Interval<int> Create(EndPoint<int> lower, EndPoint<int> upper)
        {
            return new IntInterval(lower, upper);
        }
    }
}
