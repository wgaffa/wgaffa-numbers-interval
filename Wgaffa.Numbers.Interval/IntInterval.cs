using System;

namespace Wgaffa.Numbers
{
    public class IntInterval : Interval<int>
    {
        public IntInterval(EndPoint<int> lower, EndPoint<int> upper) : base(lower, upper)
        {
        }

        public IntInterval Merge(IntInterval other)
        {
            var merged = Merge((Interval<int>)other);

            return new IntInterval(merged.Lower, merged.Upper);
        }

        public override bool Overlaps(Interval<int> other)
        {
            if (other == null)
                return false;

            var continuousLeft = (Lower.Inclusive && other.Upper.Inclusive) && Math.Abs(Lower.Value - other.Upper.Value) <= 1;
            var continuousRight = (Upper.Inclusive && other.Lower.Inclusive) && Math.Abs(Upper.Value - other.Lower.Value) <= 1;

            if (continuousLeft || continuousRight)
                return true;

            return Lower.IsInsideLowerBounds(other.Upper) && other.Lower.IsInsideLowerBounds(Upper);
        }
    }
}
